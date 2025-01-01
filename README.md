
# Steganography Image Encoder/Decoder

This project provides a simple implementation of steganography in C#. It allows you to hide text data within a PNG image using the Least Significant Bit (LSB) technique. The project includes both encoding and decoding functionalities.

## Features

- **Encode Text into Image:** Hide text data within a PNG image.
- **Decode Text from Image:** Extract hidden text data from a PNG image.

## Prerequisites

- **.NET SDK:** Ensure you have the .NET SDK installed on your machine. You can download it from [here](https://dotnet.microsoft.com/download).

## Getting Started

### Cloning the Repository

1. Clone the repository to your local machine:

   ```bash
   git clone https://github.com/unreliablecode/Steganography.git
   cd Steganography
   ```

### Building the Project

1. Build the project using the .NET CLI:

   ```bash
   dotnet build
   ```

### Running the Application

1. Run the application with the desired input and output paths:

   ```bash
   dotnet run -- --input input.png --output output.png --text "Hello, this is a secret message!"
   ```

   - `--input`: Path to the input image.
   - `--output`: Path to save the modified image.
   - `--text`: Text to hide in the image.

### Decoding the Image

1. To decode the hidden text from an image:

   ```bash
   dotnet run -- --decode --input output.png
   ```

   - `--decode`: Flag to indicate decoding mode.
   - `--input`: Path to the image containing hidden text.

## Example Usage

### Encoding

```bash
dotnet run -- --input example.png --output hidden.png --text "Secret Message"
```

### Decoding

```bash
dotnet run -- --decode --input hidden.png
```

## Notes

- **Image Size:** Ensure the input image is large enough to hide the text. The size requirement depends on the length of the text.
- **Image Format:** The current implementation supports PNG images. You can extend it to support other formats if needed.

## Contributing

Contributions are welcome! Feel free to open issues or submit pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact

- **GitHub:** [unreliablecode](https://github.com/unreliablecode)
- **Email:** [admin@unreliablecode.net](mailto:admin@unreliablecode.net)

---

Feel free to modify the README to better fit your project's needs!
```

### Additional Steps:

1. **Create a `LICENSE` File:**
   - You can use the MIT License template. Here’s a simple one:

     ```markdown
     MIT License

     Copyright (c) 2025 unreliablecode

     Permission is hereby granted, free of charge, to any person obtaining a copy
     of this software and associated documentation files (the "Software"), to deal
     in the Software without restriction, including without limitation the rights
     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
     copies of the Software, and to permit persons to whom the Software is
     furnished to do so, subject to the following conditions:

     The above copyright notice and this permission notice shall be included in all
     copies or substantial portions of the Software.

     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
     SOFTWARE.
     ```

2. **Update the `Main` Method for Command-Line Arguments:**
   - Modify the `Main` method to handle command-line arguments for input, output, and text.

Here’s an updated `Main` method to handle command-line arguments:

```csharp
using System;
using System.Drawing;

class Steganography
{
    // Existing methods (EncodeTextIntoImage, DecodeTextFromImage, HideBytes, ExtractBytes)

    static void Main(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: dotnet run -- --input <inputImagePath> --output <outputImagePath> --text <textToHide>");
            Console.WriteLine("       dotnet run -- --decode --input <inputImagePath>");
            return;
        }

        string inputImagePath = null;
        string outputImagePath = null;
        string textToHide = null;
        bool decodeMode = false;

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--input":
                    inputImagePath = args[i + 1];
                    i++;
                    break;
                case "--output":
                    outputImagePath = args[i + 1];
                    i++;
                    break;
                case "--text":
                    textToHide = args[i + 1];
                    i++;
                    break;
                case "--decode":
                    decodeMode = true;
                    break;
            }
        }

        if (decodeMode)
        {
            if (inputImagePath == null)
            {
                Console.WriteLine("Input image path is required for decoding.");
                return;
            }

            string decodedText = DecodeTextFromImage(inputImagePath);
            Console.WriteLine("Decoded text: " + decodedText);
        }
        else
        {
            if (inputImagePath == null || outputImagePath == null || textToHide == null)
            {
                Console.WriteLine("Input image path, output image path, and text to hide are required for encoding.");
                return;
            }

            EncodeTextIntoImage(inputImagePath, outputImagePath, textToHide);
            Console.WriteLine("Text encoded into the image.");
        }
    }
}
