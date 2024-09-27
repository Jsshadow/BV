using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace INFOIBV
{
    public partial class INFOIBV : Form
    {
        private Bitmap InputImage;
        private Bitmap OutputImage;

        /*
         * this enum defines the processing functions that will be shown in the dropdown (a.k.a. combobox)
         * you can expand it by adding new entries to applyProcessingFunction()
         */
        private enum ProcessingFunctions
        {
            Grayscale,
            Invert,
            AdjustContrast,
            ConvolutionFilter,
            MedianFilter,
            DetectEdges,
            Threshold,
            Pipeline1,
            Pipeline2,
            Pipeline3_1,
            Pipeline3_2,
            Pipeline3_3,
            Pipeline3_4,
            Pipeline3_5,
            Dilate,
            Erode
        }

        private enum ElementShape
        {
            Square,
            Star
        }

        /*
         * these are the parameters for your processing functions, you should add more as you see fit
         * it is useful to set them based on controls such as sliders, which you can add to the form
         */
        private byte filterSize = 5;
        private float filterSigma = 1f;
        private byte threshold = 127;
        private byte pipelineThreshold = 25;

        public INFOIBV()
        {
            InitializeComponent();
            populateCombobox();
            populateFilterSize();
            populateShapes();
        }

        /*
         * populateCombobox: populates the combobox with items as defined by the ProcessingFunctions enum
         */
        private void populateCombobox()
        {
            foreach (string itemName in Enum.GetNames(typeof(ProcessingFunctions)))
            {
                string ItemNameSpaces = Regex.Replace(Regex.Replace(itemName, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
                comboBox.Items.Add(ItemNameSpaces);
            }
            comboBox.SelectedIndex = 0;
        }

        private void populateFilterSize()
        {
            for (int i = 3; i < 15; i+=2)
            {
                FilterSize.Items.Add(i);
            }
            FilterSize.SelectedIndex = 0;
        }

        private void populateShapes()
        {
            foreach (string shape in Enum.GetNames(typeof(ElementShape)))
            {
                string _shape = Regex.Replace(Regex.Replace(shape, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
                StructuringShape.Items.Add(_shape);
            }
            StructuringShape.SelectedIndex = 0;
        }

        /*
         * loadButton_Click: process when user clicks "Load" button
         */
        private void loadImageButton_Click(object sender, EventArgs e)
        {
           if (openImageDialog.ShowDialog() == DialogResult.OK)             // open file dialog
            {
                string file = openImageDialog.FileName;                     // get the file name
                imageFileName.Text = file;                                  // show file name
                if (InputImage != null) InputImage.Dispose();               // reset image
                InputImage = new Bitmap(file);                              // create new Bitmap from file
                if (InputImage.Size.Height <= 0 || InputImage.Size.Width <= 0 ||
                    InputImage.Size.Height > 512 || InputImage.Size.Width > 512) // dimension check (may be removed or altered)
                    MessageBox.Show("Error in image dimensions (have to be > 0 and <= 512)");
                else
                    pictureBox1.Image = (Image) InputImage;                 // display input image
            }
        }

        private void comboBox_Click(object sender, EventArgs e)
        {
            if (comboBox.SelectedIndex == 14 || comboBox.SelectedIndex == 15)
            {
                FilterSize.Visible = true;
                StructuringShape.Visible = true;
                Binary.Visible = true;
            }
            else
            {
                FilterSize.Visible = false;
                StructuringShape.Visible = false;
                Binary.Visible = false;
            }
        }


        /*
         * applyButton_Click: process when user clicks "Apply" button
         */
        private void applyButton_Click(object sender, EventArgs e)
        {
            if (InputImage == null) return;                                 // get out if no input image
            if (OutputImage != null) OutputImage.Dispose();                 // reset output image
            OutputImage = new Bitmap(InputImage.Size.Width, InputImage.Size.Height); // create new output image
            Color[,] Image = new Color[InputImage.Size.Width, InputImage.Size.Height]; // create array to speed-up operations (Bitmap functions are very slow)

            // copy input Bitmap to array            
            for (int x = 0; x < InputImage.Size.Width; x++)                 // loop over columns
                for (int y = 0; y < InputImage.Size.Height; y++)            // loop over rows
                    Image[x, y] = InputImage.GetPixel(x, y);                // set pixel color in array at (x,y)

            // execute image processing steps
            byte[,] workingImage = convertToGrayscale(Image);               // convert image to grayscale
            workingImage = applyProcessingFunction(workingImage);           // processing functions

            // copy array to output Bitmap
            for (int x = 0; x < workingImage.GetLength(0); x++)             // loop over columns
                for (int y = 0; y < workingImage.GetLength(1); y++)         // loop over rows
                {
                    Color newColor = Color.FromArgb(workingImage[x, y], workingImage[x, y], workingImage[x, y]);
                    OutputImage.SetPixel(x, y, newColor);                  // set the pixel color at coordinate (x,y)
                }
            
            pictureBox2.Image = (Image)OutputImage;                         // display output image
        }

        /*
         * applyProcessingFunction: defines behavior of function calls when "Apply" is pressed
         */
        private byte[,] applyProcessingFunction(byte[,] workingImage)
        {
            sbyte[,] horizontalKernel = {{-1, 0, 1}, {-2, 0, 2}, {-1, 0, 1}};                       // Define this kernel yourself
            sbyte[,] verticalKernel = {{-1, -2, -1},{0, 0, 0},{1, 2, 1}};                         // Define this kernel yourself
            switch ((ProcessingFunctions)comboBox.SelectedIndex)
            {
                case ProcessingFunctions.Grayscale:
                    return workingImage;
                case ProcessingFunctions.Invert:
                    return invertImage(workingImage);
                case ProcessingFunctions.AdjustContrast:
                    return adjustContrast(workingImage);
                case ProcessingFunctions.ConvolutionFilter:
                    float[,] filter = createGaussianFilter(filterSize, filterSigma);
                    return convolveImage(workingImage, filter);
                case ProcessingFunctions.MedianFilter:
                    return medianFilter(workingImage, filterSize);
                case ProcessingFunctions.DetectEdges:
                    return edgeMagnitude(workingImage, horizontalKernel, verticalKernel);
                case ProcessingFunctions.Threshold:
                    return thresholdImage(workingImage, threshold);
                case ProcessingFunctions.Pipeline1:
                    return pipeline1(workingImage, filterSigma, horizontalKernel, verticalKernel, pipelineThreshold);
                case ProcessingFunctions.Pipeline2:
                    return pipeline2(workingImage, horizontalKernel, verticalKernel, pipelineThreshold);
                case ProcessingFunctions.Pipeline3_1:
                    return pipeline3_1(workingImage, horizontalKernel, verticalKernel, pipelineThreshold);
                case ProcessingFunctions.Pipeline3_2:
                    return pipeline3_2(workingImage, horizontalKernel, verticalKernel, pipelineThreshold);
                case ProcessingFunctions.Pipeline3_3:
                    return pipeline3_3(workingImage, horizontalKernel, verticalKernel, pipelineThreshold);
                case ProcessingFunctions.Pipeline3_4:
                    return pipeline3_4(workingImage, horizontalKernel, verticalKernel, pipelineThreshold);
                case ProcessingFunctions.Pipeline3_5:
                    return pipeline3_5(workingImage, horizontalKernel, verticalKernel, pipelineThreshold);
                case ProcessingFunctions.Dilate:
                    int _filterSize = (FilterSize.SelectedIndex * 2) + 3;
                    ElementShape shape = StructuringShape.SelectedIndex == 0 ? ElementShape.Square : ElementShape.Star;
                    bool _binary = Binary.Checked;
                    return intArrayToByteArray(dilateImage(byteArrayToIntArray(workingImage),
                        structuringElement(shape, _filterSize, _binary), _binary));
                case ProcessingFunctions.Erode:
                    int _filterSize1 = (FilterSize.SelectedIndex * 2) + 3;
                    ElementShape shape1 = StructuringShape.SelectedIndex == 0 ? ElementShape.Square : ElementShape.Star;
                    bool _binary1 = Binary.Checked;
                    return intArrayToByteArray(erodeImage(byteArrayToIntArray(workingImage),
                        structuringElement(shape1, _filterSize1, _binary1), _binary1));
                default:
                    return null;
            }
        }

        int[,] byteArrayToIntArray(byte[,] bytes)
        {
            return PointOperationImage(bytes, (b, i, arg3) => (int)b);
        }

        byte[,] intArrayToByteArray(int[,] ints)
        {
            return PointOperationImage(ints, (i, i1, arg3) => (byte)i);
        }


        /*
         * saveButton_Click: process when user clicks "Save" button
         */
        private void saveButton_Click(object sender, EventArgs e)
        {
            if (OutputImage == null) return;                                // get out if no output image
            if (saveImageDialog.ShowDialog() == DialogResult.OK)
                OutputImage.Save(saveImageDialog.FileName);                 // save the output image
        }


        /*
         * convertToGrayScale: convert a three-channel color image to a single channel grayscale image
         * input:   inputImage          three-channel (Color) image
         * output:                      single-channel (byte) image
         */
        private byte[,] convertToGrayscale(Color[,] inputImage)
        {
            // create temporary grayscale image of the same size as input, with a single channel
            byte[,] tempImage = new byte[inputImage.GetLength(0), inputImage.GetLength(1)];

            // setup progress bar
            progressBar.Visible = true;
            progressBar.Minimum = 1;
            progressBar.Maximum = InputImage.Size.Width * InputImage.Size.Height;
            progressBar.Value = 1;
            progressBar.Step = 1;

            // process all pixels in the image
            for (int x = 0; x < InputImage.Size.Width; x++)                 // loop over columns
                for (int y = 0; y < InputImage.Size.Height; y++)            // loop over rows
                {
                    Color pixelColor = inputImage[x, y];                    // get pixel color
                    byte average = (byte)((pixelColor.R + pixelColor.B + pixelColor.G) / 3); // calculate average over the three channels
                    tempImage[x, y] = average;                              // set the new pixel color at coordinate (x,y)
                    progressBar.PerformStep();                              // increment progress bar
                }

            progressBar.Visible = false;                                    // hide progress bar

            return tempImage;
        }


        // ====================================================================
        // ============= YOUR FUNCTIONS FOR ASSIGNMENT 1 GO HERE ==============
        // ====================================================================

        /*
         * invertImage: invert a single channel (grayscale) image
         * input:   inputImage          single-channel (byte) image
         * output:                      single-channel (byte) image
         */
        private byte[,] invertImage(byte[,] inputImage)
        {
            // create temporary grayscale image
            byte[,] tempImage = new byte[inputImage.GetLength(0), inputImage.GetLength(1)];

            tempImage = PointOperationImage(inputImage, (b, x, y) => (byte)(255 - b));
            
            return tempImage;
        }
        private A[,] PointOperationImage<T,A>(T[,] inputImage,Func<T,int,int,A> operation)
        {
            A [,] tempImage = new A [inputImage.GetLength(0), inputImage.GetLength(1)];

            for (int i = 0; i < inputImage.GetLength(0); i++) 
            { 
                for (int j = 0; j < inputImage.GetLength(1); j++)
                {
                    tempImage[i, j] = operation(inputImage[i, j],i,j);
                } 
            }
            return tempImage;
        }

        private TB FoldlFilter<TA, TB>(Func<TA, TB, TB> f,TB startV, TA[,] filter)
        {
            foreach (TA v in filter)
            {
                startV = f(v,startV);
            }
            return startV;
        }
        
        private TB FoldrFilter<TA, TB>(Func<TA, TB, TB> f,TB startV, TA[,] filter)
        {
            IEnumerable<TA> enumerableThing = filter.Cast<TA>();
            foreach (TA v in enumerableThing.Reverse())
            {
                startV = f(v,startV);
            }
            return startV;
        }

        private byte getMax(byte[,] img)
        {
            return FoldlFilter((v, a) => v > a ? v : a, Byte.MinValue, img);
        }
        private byte getMin(byte[,] img)
        {
            return FoldlFilter((v, a) => v < a ? v : a, Byte.MaxValue, img);
        }
        

        /*
         * adjustContrast: create an image with the full range of intensity values used
         * input:   inputImage          single-channel (byte) image
         * output:                      single-channel (byte) image
         */
        private byte[,] adjustContrast(byte[,] inputImage)
        {
            // create temporary grayscale image
            byte[,] tempImage = new byte[inputImage.GetLength(0), inputImage.GetLength(1)];
            byte high = getMax(inputImage);
            byte low = getMin(inputImage);
            byte min = Byte.MinValue;
            byte max = Byte.MaxValue;

            byte Ca(byte a)
            {
                byte b = (byte)(min + (a - low) * ((max - min) / (high - low)));
                return b;
            }
            
            for (int i = 0; i < tempImage.GetLength((0)); i++)
            {
                for (int j = 0; j < tempImage.GetLength(1); j++)
                {
                    tempImage[i, j] = Ca(inputImage[i, j]);
                }
            }
            
            return tempImage;
        }
        

        /*
         * createGaussianFilter: create a Gaussian filter of specific square size and with a specified sigma
         * input:   size                length and width of the Gaussian filter (only odd sizes)
         *          sigma               standard deviation of the Gaussian distribution
         * output:                      Gaussian filter
         */
        private float[,] createGaussianFilter(byte size, float sigma)
        {
            if (size % 2 == 0)
            {
                MessageBox.Show("Kernel size is not odd, please set an odd kernal size");
            }
            // create temporary grayscale image
            float[,] filter = new float[size, size];
            int lengthF = filter.GetLength(0);
            int widthF = filter.GetLength(1);
            if(lengthF % 2 == 0 || widthF == 0){throw new Exception("can not place pixel in the middle of filter");}
            for (int i = -(lengthF-1)/2; i < lengthF-(lengthF-1)/2; i++) 
            { 
                for (int j = -(widthF-1)/2; j < widthF-(widthF-1)/2; j++)
                {
                    filter[i+(lengthF-1)/2, j+(widthF-1)/2] = (float)(1/(2*Math.PI*(sigma*sigma)) * Math.Pow(Math.E, -((i * i + j * j) / (2 * (sigma * sigma)))));
                } 
            }

            return filter;
        }
        

        /*
         * convolveImage: apply linear filtering of an input image
         * input:   inputImage          single-channel (byte) image
         *          filter              linear kernel
         * output:                      single-channel (byte) image
         */
        private byte[,] convolveImage(byte[,] inputImage, float[,] filter)
        {
            // create temporary grayscale image
            byte[,] tempImage = new byte[inputImage.GetLength(0), inputImage.GetLength(1)];

            for (int u = 0; u < inputImage.GetLength(0); u++) 
            { 
                for (int v = 0; v < inputImage.GetLength(1); v++)
                {
                    tempImage[u, v] = CalcConvulation(u, v, inputImage, filter);
                } 
            }

            return tempImage;
        }

        private float Clamp(float input)
        {
            if (input < 0) return 0f;
            if (input > 255) return 255f;
            return input;
        }
        
        private byte CalcConvulation(int u,int v,byte[,] inputImage,float[,] filter)
        {
            float r = 0;
            int length = inputImage.GetLength(0);
            int width = inputImage.GetLength(1);
            int lengthF = filter.GetLength(0);
            int widthF = filter.GetLength(1);
            if(lengthF % 2 == 0 || widthF == 0){throw new Exception("can not place pixel in the middle of filter");}
            for (int i = -(lengthF-1)/2; i < lengthF-(lengthF-1)/2; i++) 
            { 
                for (int j = -(widthF-1)/2; j < widthF-(widthF-1)/2; j++)
                {
                    if(0 <= u-i && length > u-i && 0 <= v-j && width > v-j)
                    {r += inputImage[u - i, v - j] * filter[i+(lengthF-1)/2,j+(widthF-1)/2];}
                } 
            }
            return (byte)Clamp(r);
        }


        /*
         * medianFilter: apply median filtering on an input image with a kernel of specified size
         * input:   inputImage          single-channel (byte) image
         *          size                length/width of the median filter kernel
         * output:                      single-channel (byte) image
         */
        private byte[] sortedFilterKernel(byte[,] inputImage, byte size, int x, int y)
        {
            List<byte> retImg = new List<byte>(size*size);
            int range = (size - 1) / 2;
            for (int i = x-range; i < x+range ; i++)
            {
                if (i < 0) continue;
                if (i > inputImage.GetLength(0)) break;
                for (int j = y-range; j < y+range; j++)
                {
                    if (j < 0) continue;

                    if (j > inputImage.GetLength((1))) break;
                    retImg.Add(inputImage[x, y]);
                }
            }
            retImg.Sort();
            return retImg.ToArray();
        }
        
        private byte[,] medianFilter(byte[,] inputImage, byte size)
        {
            if (size % 2 == 0)
            {
                MessageBox.Show("Kernel size is not odd, please set an odd kernal size");
            }
            // create temporary grayscale image
            byte[,] tempImage = new byte[inputImage.GetLength(0), inputImage.GetLength(1)];

            byte mF(int x, int y)
            {
                byte[] kernel = sortedFilterKernel(inputImage, size, x, y);
                int median;
                int h = kernel.Length / 2;
                if (kernel.Length % 2 == 0) median = (kernel[h]+kernel[h-1]) / 2;
                else median = kernel[h];
                return ((byte)median);
            }

            for (int i = 0; i < inputImage.GetLength(0); i++)
            {
                for (int j = 0; j < inputImage.GetLength(1); j++)
                {
                    tempImage[i, j] = mF(i, j);
                }
            }

            return tempImage;
        }


        /*
         * edgeMagnitude: calculate the image derivative of an input image and a provided edge kernel
         * input:   inputImage          single-channel (byte) image
         *          horizontalKernel    horizontal edge kernel
         *          verticalKernel      vertical edge kernel
         * output:                      single-channel (byte) image
         */
        private byte[,] edgeMagnitude(byte[,] inputImage, sbyte[,] horizontalKernel, sbyte[,] verticalKernel)
        {
            // create temporary grayscale image
            byte[,] tempImage = new byte[inputImage.GetLength(0), inputImage.GetLength(1)];

            float[,] fhk = PointOperationImage(horizontalKernel, (b, x, y) => (float)0.5*b);
            float[,] fvk = PointOperationImage(verticalKernel, (b, x, y) => (float)0.5*b);
            
            byte[,] h = convolveImage(inputImage, fhk);
            byte[,] v = convolveImage(inputImage, fvk);

            for (int i = 0; i < inputImage.GetLength(0); i++)
            {
                for (int j = 0; j < inputImage.GetLength(1); j++)
                {
                    tempImage[i, j] = (byte)Math.Sqrt((h[i,j] * h[i,j]) + (v[i,j] * v[i,j]));
                }
            }

            return tempImage;
        }


        /*
         * thresholdImage: threshold a grayscale image
         * input:   inputImage          single-channel (byte) image
         * output:                      single-channel (byte) image with on/off values
         */
        private byte[,] thresholdImage(byte[,] inputImage, byte threshold)
        {
            // create temporary grayscale image
            byte[,] tempImage = new byte[inputImage.GetLength(0), inputImage.GetLength(1)];

            for (int i = 0; i < inputImage.GetLength(0); i++)
            {
                for (int j = 0; j < inputImage.GetLength(1); j++)
                {
                    if (inputImage[i, j] > threshold) tempImage[i, j] = byte.MaxValue;
                    else tempImage[i, j] = byte.MinValue;
                } 
            }
            
            return tempImage;
        }

        private byte[,] pipeline1(byte[,] inputImage, float sigma, sbyte[,] horizontalKernel, sbyte[,] verticalKernel, byte threshold)
        {
            float[,] filter = createGaussianFilter(filterSize, sigma);

            byte[,] filteredImage = convolveImage(inputImage, filter);

            byte[,] edgedImage = edgeMagnitude(filteredImage, horizontalKernel, verticalKernel);

            byte[,] thresholdedImage = thresholdImage(edgedImage, threshold);

            return thresholdedImage;
        }
        
        private byte[,] pipeline2(byte[,] inputImage, sbyte[,] horizontalKernel, sbyte[,] verticalKernel, byte threshold)
        {
            byte[,] filteredImage = medianFilter(inputImage, 5);

            byte[,] edgedImage = edgeMagnitude(filteredImage, horizontalKernel, verticalKernel);

            byte[,] thresholdedImage = thresholdImage(edgedImage, threshold);

            return thresholdedImage;
        }

        private byte[,] pipeline3_1(byte[,] inputImage, sbyte[,] horizontalKernel, sbyte[,] verticalKernel,
            byte threshold)
        {
            byte[,] filteredImage = convolveImage(inputImage, createGaussianFilter(3, 1f));
            byte[,] edgedImage = edgeMagnitude(filteredImage, horizontalKernel, verticalKernel);
            byte[,] thresholdedImage = thresholdImage(edgedImage, threshold);
            return thresholdedImage;
        }
        private byte[,] pipeline3_2(byte[,] inputImage, sbyte[,] horizontalKernel, sbyte[,] verticalKernel,
            byte threshold)
        {
            byte[,] filteredImage = convolveImage(inputImage, createGaussianFilter(5, 1f));
            byte[,] edgedImage = edgeMagnitude(filteredImage, horizontalKernel, verticalKernel);
            byte[,] thresholdedImage = thresholdImage(edgedImage, threshold);
            return thresholdedImage;
        }
        private byte[,] pipeline3_3(byte[,] inputImage, sbyte[,] horizontalKernel, sbyte[,] verticalKernel,
            byte threshold)
        {
            byte[,] filteredImage = convolveImage(inputImage, createGaussianFilter(7, 1f));
            byte[,] edgedImage = edgeMagnitude(filteredImage, horizontalKernel, verticalKernel);
            byte[,] thresholdedImage = thresholdImage(edgedImage, threshold);
            return thresholdedImage;
        }
        private byte[,] pipeline3_4(byte[,] inputImage, sbyte[,] horizontalKernel, sbyte[,] verticalKernel,
            byte threshold)
        {
            byte[,] filteredImage = convolveImage(inputImage, createGaussianFilter(9, 1f));
            byte[,] edgedImage = edgeMagnitude(filteredImage, horizontalKernel, verticalKernel);
            byte[,] thresholdedImage = thresholdImage(edgedImage, threshold);
            return thresholdedImage;
        }
        private byte[,] pipeline3_5(byte[,] inputImage, sbyte[,] horizontalKernel, sbyte[,] verticalKernel,
            byte threshold)
        {
            byte[,] filteredImage = convolveImage(inputImage, createGaussianFilter(11, 1f));
            byte[,] edgedImage = edgeMagnitude(filteredImage, horizontalKernel, verticalKernel);
            byte[,] thresholdedImage = thresholdImage(edgedImage, threshold);
            return thresholdedImage;
        }

        
        // ====================================================================
        // ============= YOUR FUNCTIONS FOR ASSIGNMENT 2 GO HERE ==============
        // ====================================================================

        private int[,] andImage(int[,] image1, int[,] image2)
        {
            if ((image1.GetLength(0) != image2.GetLength(0) || (image1.GetLength(1) != image2.GetLength(1))))
            {
                MessageBox.Show("Images are not the same size");
                return (new int[0,0]);
            }

            bool intToBool(int value)
            {
                if (value > threshold) return true;
                return false;
            }
            int boolToInt(bool value)
            {
                if (value) return 255;
                return 0;
            }

            bool[,] boolImg1 = PointOperationImage(image1, (b, i, j) => intToBool(b));
            bool[,] boolImg2 = PointOperationImage(image2, (b, i, j) => intToBool(b));
            bool[,] boolImgF = new bool[boolImg1.GetLength(0), boolImg1.GetLength(1)];
            for (int i = 0; i < boolImg1.GetLength(0); i++)
            {
                for (int j = 0; j < boolImg1.GetLength(1); j++)
                {
                    boolImgF[i, j] = boolImg1[i, j] && boolImg2[i, j];
                }
            }

            return PointOperationImage(boolImgF, (b, i, j) => boolToInt(b));
        }
        
        private int[,] orImage(int[,] image1, int[,] image2)
        {
            if ((image1.GetLength(0) != image2.GetLength(0) || (image1.GetLength(1) != image2.GetLength(1))))
            {
                MessageBox.Show("Images are not the same size");
                return (new int[0,0]);
            }

            bool intToBool(int value)
            {
                if (value > threshold) return true;
                return false;
            }
            int boolToInt(bool value)
            {
                if (value) return 255;
                return 0;
            }

            bool[,] boolImg1 = PointOperationImage(image1, (b, i, j) => intToBool(b));
            bool[,] boolImg2 = PointOperationImage(image2, (b, i, j) => intToBool(b));
            bool[,] boolImgF = new bool[boolImg1.GetLength(0), boolImg1.GetLength(1)];
            for (int i = 0; i < boolImg1.GetLength(0); i++)
            {
                for (int j = 0; j < boolImg1.GetLength(1); j++)
                {
                    boolImgF[i, j] = boolImg1[i, j] || boolImg2[i, j];
                }
            }

            return PointOperationImage(boolImgF, (b, i, j) => boolToInt(b));
        }

        //max of image one plus image two (for edges ignore parts that fall outside the range)
        private int[,] maxImage(int[,] image1, int[,] image2)
        {
            return new int[0, 0];
        }
        //for min:
        //smallest difference between image1[i,j] and image2[i,j]

        private int[,] structuringElement(ElementShape shape, int size, bool binary)
        {
            if (size % 2 == 0)
            {
                MessageBox.Show("Element Size is even, please enter an odd element size");
                return new int[0, 0];
            }
            int x = (size - 3) / 2;
            int max = 255;
            int min = 0;
            int[,] square = binary ? new [,] {{max,max,max},{max,max,max},{max,max,max}} : new [,] {{1,1,1},{1,1,1},{1,1,1}};
            int[,] star = binary ? new [,] {{min,max,min}, {max,max,max}, {min,max,min}} : new [,] {{-1,1,-1},{1,1,1},{-1,1,-1}};
            switch (shape)
            {
                case ElementShape.Square:
                    return square;
                case ElementShape.Star:
                    return star;
                default:
                    MessageBox.Show("Please enter a valid Element Shape");
                    return new int[0, 0];
            }
        }

        private int[,] setZeros(int h, int w)
        {
            int[,] outimg = new int[w,h];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    outimg[i, j] = 0;
                }
            }
            return outimg;
        }
        private int[,] set255(int h, int w)
        {
            int[,] outimg = new int[w, h];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    outimg[i, j] = 255;
                }
            }
            return outimg;
        }

        private int[,] dilateImage(int[,] inputImage, int[,] structuralElement, bool binary)
        {
            int size = structuralElement.GetLength(0);
            int x = (size - 1) / 2;
            int[,] outputImage = setZeros(inputImage.GetLength(1), inputImage.GetLength(0));
            if (binary)
            {
                for (int i = 0; i < inputImage.GetLength(0); i++)
                {
                    for (int j = 0; j < inputImage.GetLength(1); j++)
                    {
                        if (inputImage[i, j] == 0) break;
                        
                        for (int k = -x; k <= x; k++)
                        {
                            for (int l = -x; l <= x; l++)
                            {
                                if (i + k < 0 || i + k >= inputImage.GetLength(0))
                                {
                                    break;
                                }
                                if (j + l < 0 || j + l >= inputImage.GetLength(1))
                                {
                                    break;
                                }
                                if (structuralElement[k+x, l+x] == 255)
                                {
                                    outputImage[i + k, j + l] = 255;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < inputImage.GetLength(0); i++)
                {
                    for (int j = 0; j < inputImage.GetLength(1); j++)
                    {
                        int[,] temp = setZeros(structuralElement.GetLength(0), structuralElement.GetLength(1));
                        for (int k = -x; k <= x; k++)
                        {
                            for (int l = -x; l <= x; l++)
                            {
                                if (i + k < 0 || i + k >= inputImage.GetLength(0))
                                {
                                    break;
                                }
                                if (j + l < 0 || j + l >= inputImage.GetLength(1))
                                {
                                    break;
                                }
                                if (structuralElement[k+x, l+x] == -1) break;
                                temp[k+x, l+x] = inputImage[i + k, j + l] + structuralElement[k+x, l+x];
                            }
                        }
                        outputImage[i, j] = (int)getMax(PointOperationImage(temp, (i1, I, J) => (byte)i1));
                    }
                }
            }
            return outputImage;
        }

        private int[,] erodeImage(int[,] inputImage, int[,] structuralElement, bool binary)
        {
            int size = structuralElement.GetLength(0);
            int x = (size - 1) / 2;
            int[,] outputImage = setZeros(inputImage.GetLength(1), inputImage.GetLength(0));
            if (binary)
            {
                for (int i = 0; i < inputImage.GetLength(0); i++)
                {
                    for (int j = 0; j < inputImage.GetLength(1); j++)
                    {
                        bool check = true;
                        for (int k = -x; k <= x; k++)
                        {
                            for (int l = -x; l <= x; l++)
                            {
                                if (i + k < 0 || i + k >= inputImage.GetLength(0))
                                {
                                    break;
                                }
                                if (j + l < 0 || j + l >= inputImage.GetLength(1))
                                {
                                    break;
                                }
                                check = ((inputImage[i + k, j + l] == 255 && structuralElement[k+x, l+x] == 255)|| (inputImage[i+k, j+l] == 0 && structuralElement[k+x,l+x] == 0)) && check;
                            }
                        }
                        if (check)
                        {
                            outputImage[i, j] = 255;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < inputImage.GetLength(0); i++)
                {
                    for (int j = 0; j < inputImage.GetLength(1); j++)
                    {
                        int[,] temp = set255(structuralElement.GetLength(0), structuralElement.GetLength(1));
                        for (int k = -x; k <= x; k++)
                        {
                            for (int l = -x; l <= x; l++)
                            {
                                if (i + k < 0 || i + k >= inputImage.GetLength(0))
                                {
                                    break;
                                }
                                if (j + l < 0 || j + l >= inputImage.GetLength(1))
                                {
                                    break;
                                }
                                if (structuralElement[k+x, l+x] == -1) break;
                                temp[k+x, l+x] = inputImage[i + k, j + l] - structuralElement[k+x, l+x];
                            }
                        }
                        outputImage[i, j] = (int)getMin(PointOperationImage(temp, (i1, I, J) => (byte)i1));
                    }
                }
            }
            return outputImage;
        }
        

        // ====================================================================
        // ============= YOUR FUNCTIONS FOR ASSIGNMENT 3 GO HERE ==============
        // ====================================================================
    }
}