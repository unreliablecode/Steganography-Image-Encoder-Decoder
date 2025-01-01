using System;
using System.Drawing;
using System.IO;

class Steganography
{
    public static void EncodeTextIntoImage(string inputImagePath, string outputImagePath, string textToHide)
    {
        // Load the image
        using (Bitmap bitmap = new Bitmap(inputImagePath))
        {
            // Convert the text to a byte array
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(textToHide);
            int textLength = textBytes.Length;

            // Check if the image has enough pixels to hide the text
            if (bitmap.Width * bitmap.Height * 3 < textLength * 8)
            {
                throw new Exception("The image is too small to hide the text.");
            }

            // Convert the length of the text to bytes and hide it first
            byte[] lengthBytes = BitConverter.GetBytes(textLength);
            HideBytes(bitmap, lengthBytes, 0);

            // Hide the actual text bytes
            HideBytes(bitmap, textBytes, lengthBytes.Length * 8);

            // Save the modified image
            bitmap.Save(outputImagePath, System.Drawing.Imaging.ImageFormat.Png);
        }
    }

    private static void HideBytes(Bitmap bitmap, byte[] bytesToHide, int startBit)
    {
        int bitIndex = startBit;
        for (int i = 0; i < bytesToHide.Length; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                int x = bitIndex / (bitmap.Height * 3);
                int y = (bitIndex % (bitmap.Height * 3)) / 3;
                int colorIndex = bitIndex % 3;

                Color pixelColor = bitmap.GetPixel(x, y);
                int colorValue = colorIndex switch
                {
                    0 => pixelColor.R,
                    1 => pixelColor.G,
                    2 => pixelColor.B,
                    _ => throw new InvalidOperationException("Invalid color index")
                };

                // Set the least significant bit
                colorValue = (colorValue & ~1) | ((bytesToHide[i] >> j) & 1);

                // Set the modified color back to the pixel
                switch (colorIndex)
                {
                    case 0:
                        bitmap.SetPixel(x, y, Color.FromArgb(colorValue, pixelColor.G, pixelColor.B));
                        break;
                    case 1:
                        bitmap.SetPixel(x, y, Color.FromArgb(pixelColor.R, colorValue, pixelColor.B));
                        break;
                    case 2:
                        bitmap.SetPixel(x, y, Color.FromArgb(pixelColor.R, pixelColor.G, colorValue));
                        break;
                }

                bitIndex++;
            }
        }
    }

    public static string DecodeTextFromImage(string inputImagePath)
    {
        // Load the image
        using (Bitmap bitmap = new Bitmap(inputImagePath))
        {
            // Read the length of the hidden text
            byte[] lengthBytes = new byte[4];
            ExtractBytes(bitmap, lengthBytes, 0);
            int textLength = BitConverter.ToInt32(lengthBytes, 0);

            // Read the hidden text
            byte[] textBytes = new byte[textLength];
            ExtractBytes(bitmap, textBytes, lengthBytes.Length * 8);

            // Convert the bytes to a string
            return System.Text.Encoding.UTF8.GetString(textBytes);
        }
    }

    private static void ExtractBytes(Bitmap bitmap, byte[] bytesToExtract, int startBit)
    {
        int bitIndex = startBit;
        for (int i = 0; i < bytesToExtract.Length; i++)
        {
            bytesToExtract[i] = 0;
            for (int j = 0; j < 8; j++)
            {
                int x = bitIndex / (bitmap.Height * 3);
                int y = (bitIndex % (bitmap.Height * 3)) / 3;
                int colorIndex = bitIndex % 3;

                Color pixelColor = bitmap.GetPixel(x, y);
                int colorValue = colorIndex switch
                {
                    0 => pixelColor.R,
                    1 => pixelColor.G,
                    2 => pixelColor.B,
                    _ => throw new InvalidOperationException("Invalid color index")
                };

                // Extract the least significant bit
                bytesToExtract[i] |= (byte)((colorValue & 1) << j);

                bitIndex++;
            }
        }
    }

    static void Main(string[] args)
    {
        string inputImagePath = "input.png";
        string outputImagePath = "output.png";
        string textToHide = "Hello, this is a secret message!";

        // Encode the text into the image
        EncodeTextIntoImage(inputImagePath, outputImagePath, textToHide);
        Console.WriteLine("Text encoded into the image.");

        // Decode the text from the image
        string decodedText = DecodeTextFromImage(outputImagePath);
        Console.WriteLine("Decoded text: " + decodedText);
    }
}
