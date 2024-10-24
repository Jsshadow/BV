﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms.VisualStyles;

namespace INFOIBV
{
    public partial class INFOIBV : Form
    {
        private Bitmap InputImage;
        private Bitmap InputImage2;
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
            And,
            Or,
            Dilate,
            Erode,
            Open,
            Close,
            CountValues,
            TraceBoundary,
            HoughTransform,
            peakfinding,
            houghLineDetection,
            HoughTransformAngleLimits
        }

        public enum ElementShape
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
            populateTextBoxes();
        }

        /*
         * populateCombobox: populates the combobox with items as defined by the ProcessingFunctions enum
         */
        private void populateCombobox()
        {
            foreach (string itemName in Enum.GetNames(typeof(ProcessingFunctions)))
            {
                string ItemNameSpaces = Regex.Replace(Regex.Replace(itemName, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"),
                    @"(\p{Ll})(\P{Ll})", "$1 $2");
                comboBox.Items.Add(ItemNameSpaces);
            }

            comboBox.SelectedIndex = 0;
        }

        private void populateTextBoxes()
        {
            MaxGap.Text = Convert.ToString(maxG);
            minLength.Text = Convert.ToString(minL);
            EdgeThreshold.Text = Convert.ToString(mit);
            peakingtheshold.Text = Convert.ToString(peakThreshold);
            minAngleText.Text = Convert.ToString(0);
            maxAngleText.Text = Convert.ToString(360);
        }

        private void populateFilterSize()
        {
            for (int i = 3; i < 56; i += 2)
            {
                FilterSize.Items.Add(i);
            }

            FilterSize.SelectedIndex = 0;
        }

        private void populateShapes()
        {
            foreach (string shape in Enum.GetNames(typeof(ElementShape)))
            {
                string _shape = Regex.Replace(Regex.Replace(shape, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"),
                    @"(\p{Ll})(\P{Ll})", "$1 $2");
                StructuringShape.Items.Add(_shape);
            }

            StructuringShape.SelectedIndex = 0;
        }

        /*
         * loadButton_Click: process when user clicks "Load" button
         */
        private void loadImageButton_Click(object sender, EventArgs e)
        {
            if (openImageDialog.ShowDialog() == DialogResult.OK) // open file dialog
            {
                string file = openImageDialog.FileName; // get the file name
                imageFileName.Text = file; // show file name
                if (InputImage != null) InputImage.Dispose(); // reset image
                InputImage = new Bitmap(file); // create new Bitmap from file
                if (InputImage.Size.Height <= 0 || InputImage.Size.Width <= 0 ||
                    InputImage.Size.Height > 512 ||
                    InputImage.Size.Width > 512) // dimension check (may be removed or altered)
                    MessageBox.Show("Error in image dimensions (have to be > 0 and <= 512)");
                else
                    pictureBox1.Image = (Image)InputImage; // display input image
            }
        }

        private void loadImageButton2_Click(object sender, EventArgs e)
        {
            if (openImageDialog.ShowDialog() == DialogResult.OK) // open file dialog
            {
                string file = openImageDialog.FileName; // get the file name
                imageFileName.Text = file; // show file name
                if (InputImage2 != null) InputImage2.Dispose(); // reset image
                InputImage2 = new Bitmap(file); // create new Bitmap from file
                if (InputImage2.Size.Height <= 0 || InputImage2.Size.Width <= 0 ||
                    InputImage2.Size.Height > 512 ||
                    InputImage2.Size.Width > 512) // dimension check (may be removed or altered)
                    MessageBox.Show("Error in image dimensions (have to be > 0 and <= 512)");
                //else
                //pictureBox2.Image = (Image) InputImage2;                 // display input image
            }
        }
        
        private void Binary_CheckedChanged(object sender, EventArgs e)
        {
            if ((string)comboBox.SelectedItem == "hough Line Detection" && !Binary.Checked)
            {
                EdgeThreshold.Visible = true;
                label3.Visible = true;
            }
            else
            {
                EdgeThreshold.Visible = false;
                label3.Visible = false;
            }
        }
        

        private void comboBox_Click(object sender, EventArgs e)
        {
            FilterSize.Visible = false;
            StructuringShape.Visible = false;
            Binary.Visible = false;
            LoadImage2.Visible = false;
            MaxGap.Visible = false;
            minLength.Visible = false;
            EdgeThreshold.Visible = false;
            peakingtheshold.Visible = false;
            minAngleText.Visible = false;
            maxAngleText.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;

            if (comboBox.SelectedIndex == 16 || comboBox.SelectedIndex == 17 || comboBox.SelectedIndex == 18 ||
                comboBox.SelectedIndex == 19)
            {
                FilterSize.Visible = true;
                StructuringShape.Visible = true;
                Binary.Visible = true;
                LoadImage2.Visible = false;
            }
            else if (comboBox.SelectedIndex == 14 || comboBox.SelectedIndex == 15)
            {
                FilterSize.Visible = false;
                StructuringShape.Visible = false;
                Binary.Visible = false;
                LoadImage2.Visible = true;
            }
            if (comboBox.SelectedIndex == 23 || comboBox.SelectedIndex == 22)
            {
                Binary.Visible = true; 
            }
            if ((string)comboBox.SelectedItem == "hough Line Detection")
            {
                Binary.Visible = true; 
                MaxGap.Visible = true;
                minLength.Visible = true;
                peakingtheshold.Visible = true;
                label1.Visible = true;
                label2.Visible = true;
                label4.Visible = true;
                if (!Binary.Checked)
                {
                    EdgeThreshold.Visible = true;
                    label3.Visible = true;
                }
            }
            if ((string)comboBox.SelectedItem == "peakfinding")
            {
                peakingtheshold.Visible = true;
                label4.Visible = true;
            }

            if ((string)comboBox.SelectedItem == "Hough Transform Angle Limits")
            {
                label5.Visible = true;
                label6.Visible = true;
                minAngleText.Visible = true;
                maxAngleText.Visible = true;
                Binary.Visible = true;
            }
        }


        /*
         * applyButton_Click: process when user clicks "Apply" button
         */
        private void applyButton_Click(object sender, EventArgs e)
        {
            if (comboBox.SelectedIndex == 14 || comboBox.SelectedIndex == 15)
            {
                if (InputImage == null || InputImage2 == null) return; // get out if no input image
                if (OutputImage != null) OutputImage.Dispose(); // reset output image
                OutputImage = new Bitmap(InputImage.Size.Width, InputImage.Size.Height); // create new output image
                Color[,]
                    Image1 = new Color[InputImage.Size.Width,
                        InputImage.Size.Height]; // create array to speed-up operations (Bitmap functions are very slow)
                Color[,] Image2 = new Color[InputImage2.Size.Width, InputImage2.Size.Height];

                // copy input Bitmap to array            
                for (int x = 0; x < InputImage.Size.Width; x++) // loop over columns
                for (int y = 0; y < InputImage.Size.Height; y++) // loop over rows
                    Image1[x, y] = InputImage.GetPixel(x, y); // set pixel color in array at (x,y)

                for (int x = 0; x < InputImage2.Size.Width; x++)
                for (int y = 0; y < InputImage2.Size.Height; y++)
                    Image2[x, y] = InputImage2.GetPixel(x, y);

                // execute image processing steps
                byte[,] workingImage = convertToGrayscale(Image1); // convert image to grayscale
                byte[,] workingImage2 = convertToGrayscale(Image2);
                workingImage = applyProcessingFunction(workingImage, workingImage2); // processing functions


                // copy array to output Bitmap
                for (int x = 0; x < workingImage.GetLength(0); x++) // loop over columns
                for (int y = 0; y < workingImage.GetLength(1); y++) // loop over rows
                {
                    Color newColor = Color.FromArgb(workingImage[x, y], workingImage[x, y], workingImage[x, y]);
                    OutputImage.SetPixel(x, y, newColor); // set the pixel color at coordinate (x,y)
                }

                pictureBox2.Image = (Image)OutputImage; // display output image
            }
            else
            {
                if (InputImage == null) return; // get out if no input image
                if (OutputImage != null) OutputImage.Dispose(); // reset output image
                OutputImage = (string)comboBox.SelectedItem == "Hough Transform" || (string)comboBox.SelectedItem == "peakfinding" || (string)comboBox.SelectedItem == "Hough Transform Angle Limits"
                    ? new Bitmap(512, 512)
                    : new Bitmap(InputImage.Width, InputImage.Height); // create new output image
                Color[,]
                    Image1 = new Color[InputImage.Size.Width,
                        InputImage.Size.Height]; // create array to speed-up operations (Bitmap functions are very slow)

                // copy input Bitmap to array            
                for (int x = 0; x < InputImage.Size.Width; x++) // loop over columns
                for (int y = 0; y < InputImage.Size.Height; y++) // loop over rows
                    Image1[x, y] = InputImage.GetPixel(x, y); // set pixel color in array at (x,y)

                // execute image processing steps
                byte[,] workingImage = convertToGrayscale(Image1); // convert image to grayscale
                
                if ((string)comboBox.SelectedItem != "hough Line Detection")
                {
                    workingImage = applyProcessingFunction(workingImage, new byte[0, 0]);//procces image
                    // copy array to output Bitmap
                    for (int x = 0; x < workingImage.GetLength(0) && x < OutputImage.Size.Width; x++) // loop over columns
                    for (int y = 0; y < workingImage.GetLength(1) && y < OutputImage.Size.Height; y++) // loop over rows
                    {
                        Color newColor = Color.FromArgb(workingImage[x, y], workingImage[x, y], workingImage[x, y]);
                        OutputImage.SetPixel(x, y, newColor); // set the pixel color at coordinate (x,y)
                    }
                }
                else //add lines on top of image
                {
                    OutputImage = (Bitmap)InputImage.Clone();
                    workingImage = applyProcessingFunction(workingImage, new byte[0, 0]);//create image with lines only
                    for (int x = 0; x < workingImage.GetLength(0) && x < OutputImage.Size.Width; x++) // loop over columns
                    for (int y = 0; y < workingImage.GetLength(1) && y < OutputImage.Size.Height; y++) // loop over rows
                    {
                        if (workingImage[x, y] > 0)
                        {
                            Color newColor = Color.FromArgb(workingImage[x, y], 0, 0);
                            OutputImage.SetPixel(x, y, newColor); // set the pixel color at coordinate (x,y)
                        }
                    } 
                }
                pictureBox2.Image = (Image)OutputImage; // display output image
            }
        }

        /*
         * applyProcessingFunction: defines behavior of function calls when "Apply" is pressed
         */
        private byte[,] applyProcessingFunction(byte[,] workingImage, byte[,] workingImage2)
        {
            sbyte[,] horizontalKernel = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } }; // Define this kernel yourself
            sbyte[,] verticalKernel = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } }; // Define this kernel yourself
            sbyte[,] horizontalKernelR = { { 1, 0, -1 }, { 2, 0, -2 }, { 1, 0, -1 } };
            sbyte[,] verticalKernelR = { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };
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
                    return edgeMagnitude(workingImage, horizontalKernel, verticalKernel, horizontalKernelR,
                        verticalKernelR);
                case ProcessingFunctions.Threshold:
                    return thresholdImage(workingImage, 10);
                /*
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
                    */
                case ProcessingFunctions.And:
                    return (new BinaryImage(thresholdImage(workingImage, threshold)) &&
                            new BinaryImage(thresholdImage(workingImage2, threshold))).ToByteArray();
                case ProcessingFunctions.Or:
                    return (new BinaryImage(thresholdImage(workingImage, threshold)) ||
                            new BinaryImage(thresholdImage(workingImage2, threshold))).ToByteArray();
                case ProcessingFunctions.Dilate:
                    int _filterSize = (FilterSize.SelectedIndex * 2) + 3;
                    ElementShape shape = StructuringShape.SelectedIndex == 0 ? ElementShape.Square : ElementShape.Star;
                    StructuringElement H = new StructuringElement(shape, _filterSize);
                    return Binary.Checked
                        ? (new BinaryImage(workingImage) + H).ToByteArray()
                        : (new GrayScaleImage(workingImage) + H).ToByteArray();

                case ProcessingFunctions.Erode:
                    int _filterSize2 = (FilterSize.SelectedIndex * 2) + 3;
                    ElementShape shape2 = StructuringShape.SelectedIndex == 0 ? ElementShape.Square : ElementShape.Star;
                    StructuringElement H2 = new StructuringElement(shape2, _filterSize2);
                    return Binary.Checked
                        ? (new BinaryImage(thresholdImage(workingImage, threshold)) - H2).ToByteArray()
                        : (new GrayScaleImage(workingImage) - H2).ToByteArray();
                case ProcessingFunctions.Open:
                    int _filterSize3 = (FilterSize.SelectedIndex * 2) + 3;
                    ElementShape shape3 = StructuringShape.SelectedIndex == 0 ? ElementShape.Square : ElementShape.Star;
                    StructuringElement H3 = new StructuringElement(shape3, _filterSize3);
                    return Binary.Checked
                        ? (new BinaryImage(thresholdImage(workingImage, threshold)) * H3).ToByteArray()
                        : (new GrayScaleImage(workingImage) * H3).ToByteArray();
                case ProcessingFunctions.Close:
                    int _filterSize4 = (FilterSize.SelectedIndex * 2) + 3;
                    ElementShape shape4 = StructuringShape.SelectedIndex == 0 ? ElementShape.Square : ElementShape.Star;
                    StructuringElement H4 = new StructuringElement(shape4, _filterSize4);
                    return Binary.Checked
                        ? (new BinaryImage(thresholdImage(workingImage, threshold)) / H4).ToByteArray()
                        : (new GrayScaleImage(workingImage) / H4).ToByteArray();
                case ProcessingFunctions.CountValues:
                    GrayScaleImage I = new GrayScaleImage(workingImage);
                    var c = I.countValues();
                    Console.WriteLine(c.Item1);
                    foreach (var pair in c.Item2)
                    {
                        Console.WriteLine(pair);
                    }

                    return I.ToByteArray();
                case ProcessingFunctions.TraceBoundary:
                    BinaryImage I2 = new BinaryImage(workingImage);
                    return I2.traceBoundary().ToByteArray();
                case ProcessingFunctions.HoughTransform:
                    return Binary.Checked
                        ? new BinaryImage(workingImage).HoughTransform().ToByteArray()
                        : new GrayScaleImage(workingImage).HoughTransform().ToByteArray();
                case ProcessingFunctions.peakfinding:
                    peakThreshold = Convert.ToDouble(peakingtheshold.Text);
                    return Binary.Checked
                        ? new BinaryImage(workingImage).peakFind().ToByteArray()
                        : new GrayScaleImage(workingImage).peakFind().ToByteArray();
                case ProcessingFunctions.houghLineDetection:
                    peakThreshold = Convert.ToDouble(peakingtheshold.Text);
                    mit = Convert.ToInt32(EdgeThreshold.Text);
                    maxG = Convert.ToInt32(MaxGap.Text);
                    minL = Convert.ToInt32(minLength.Text);
                    return Binary.Checked
                        ? new BinaryImage(workingImage).visualizeHoughLineSegments()
                        : new GrayScaleImage(workingImage).visualizeHoughLineSegments();
                case ProcessingFunctions.HoughTransformAngleLimits:
                    int minA = Convert.ToInt32(minAngleText.Text);
                    int maxA = Convert.ToInt32(maxAngleText.Text);
                    return Binary.Checked
                        ? new BinaryImage(workingImage).houghTransformAngleLimits(minA,maxA).ToByteArray()
                        : new GrayScaleImage(workingImage).houghTransformAngleLimits(minA,maxA).ToByteArray();
                default:
                    return null;
            }
        }

        int[,] byteArrayToIntArray(byte[,] bytes)
        {
            return map2D(bytes, b => (int)b);
        }

        byte[,] intArrayToByteArray(int[,] ints)
        {
            return map2D(ints, i => (byte)i);
        }


        /*
         * saveButton_Click: process when user clicks "Save" button
         */
        private void saveButton_Click(object sender, EventArgs e)
        {
            if (OutputImage == null) return; // get out if no output image
            if (saveImageDialog.ShowDialog() == DialogResult.OK)
                OutputImage.Save(saveImageDialog.FileName); // save the output image
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
            for (int x = 0; x < InputImage.Size.Width; x++) // loop over columns
            for (int y = 0; y < InputImage.Size.Height; y++) // loop over rows
            {
                Color pixelColor = inputImage[x, y]; // get pixel color
                byte average =
                    (byte)((pixelColor.R + pixelColor.B + pixelColor.G) /
                           3); // calculate average over the three channels
                tempImage[x, y] = average; // set the new pixel color at coordinate (x,y)
                progressBar.PerformStep(); // increment progress bar
            }

            progressBar.Visible = false; // hide progress bar

            return tempImage;
        }

        //user definded classes and generic functions
        private static A[,] map2dIndexed<T, A>(T[,] inputImage, Func<T, int, int, A> operation)
        {
            A[,] tempImage = new A [inputImage.GetLength(0), inputImage.GetLength(1)];

            for (int i = 0; i < inputImage.GetLength(0); i++)
            {
                for (int j = 0; j < inputImage.GetLength(1); j++)
                {
                    tempImage[i, j] = operation(inputImage[i, j], i, j);
                }
            }

            return tempImage;
        }

        private static A[,] map2D<T, A>(T[,] inputImage, Func<T, A> operation)
        {
            A[,] tempImage = new A [inputImage.GetLength(0), inputImage.GetLength(1)];

            for (int i = 0; i < inputImage.GetLength(0); i++)
            {
                for (int j = 0; j < inputImage.GetLength(1); j++)
                {
                    tempImage[i, j] = operation(inputImage[i, j]);
                }
            }

            return tempImage;
        }

        private static TB Foldl2D<TA, TB>(Func<TA, TB, TB> f, TB startV, TA[,] filter)
        {
            foreach (TA v in filter)
            {
                startV = f(v, startV);
            }

            return startV;
        }

        private static TB Foldl2DIndexed<TA, TB>(Func<TA, int, int, TB, TB> f, TB startV, TA[,] filter)
        {
            for (int i = 0; i < filter.GetLength(0); i++)
            {
                for (int j = 0; j < filter.GetLength(1); j++)
                {
                    startV = f(filter[i, j], i, j, startV);
                }
            }

            return startV;
        }

        private static TB Foldr2D<TA, TB>(Func<TA, TB, TB> f, TB startV, TA[,] filter)
        {
            IEnumerable<TA> enumerableThing = filter.Cast<TA>();
            foreach (TA v in enumerableThing.Reverse())
            {
                startV = f(v, startV);
            }

            return startV;
        }

        private static IEnumerable<(int x, int y)> EnumerableRange2D(int a, int b)
        {
            return from u in Enumerable.Range(0, a) from v in Enumerable.Range(0, b) select (u, v);
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
            GrayScaleImage tempImage = new GrayScaleImage(new byte[inputImage.GetLength(0), inputImage.GetLength(1)]);
            tempImage.Apply(p => 255 - p);
            return tempImage.ToByteArray();
        }

        /*
         * adjustContrast: create an image with the full range of intensity values used
         * input:   inputImage          single-channel (byte) image
         * output:                      single-channel (byte) image
         */
        private byte[,] adjustContrast(byte[,] inputImage)
        {
            // create temporary grayscale image
            GrayScaleImage tempImage = new GrayScaleImage(new byte[inputImage.GetLength(0), inputImage.GetLength(1)]);
            //byte high = tempImage.getMax(inputImage);
            byte low = tempImage.getMin(inputImage);
            byte min = Byte.MinValue;
            byte max = Byte.MaxValue;

            byte Ca(byte a)
            {
                //byte b = (byte)(min + (a - low) * ((max - min) / (high - low)));
                return a;
            }

            for (int i = 0; i < tempImage.GetLength((0)); i++)
            {
                for (int j = 0; j < tempImage.GetLength(1); j++)
                {
                    tempImage[i, j] = Ca(inputImage[i, j]);
                }
            }

            return tempImage.ToByteArray();
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
            if (lengthF % 2 == 0 || widthF == 0)
            {
                throw new Exception("can not place pixel in the middle of filter");
            }

            for (int i = -(lengthF - 1) / 2; i < lengthF - (lengthF - 1) / 2; i++)
            {
                for (int j = -(widthF - 1) / 2; j < widthF - (widthF - 1) / 2; j++)
                {
                    filter[i + (lengthF - 1) / 2, j + (widthF - 1) / 2] = (float)(1 / (2 * Math.PI * (sigma * sigma)) *
                        Math.Pow(Math.E, -((i * i + j * j) / (2 * (sigma * sigma)))));
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
            GrayScaleImage tempImage = new GrayScaleImage(new Byte[inputImage.GetLength(0), inputImage.GetLength(1)]);

            for (int u = 0; u < inputImage.GetLength(0); u++)
            {
                for (int v = 0; v < inputImage.GetLength(1); v++)
                {
                    tempImage[u, v] = CalcConvulation(u, v, inputImage, filter);
                }
            }

            return tempImage.ToByteArray();
        }

        private float Clamp(float input)
        {
            if (input < 0) return 0f;
            if (input > 255) return 255f;
            return input;
        }

        private byte CalcConvulation(int u, int v, byte[,] inputImage, float[,] filter)
        {
            float r = 0;
            int length = inputImage.GetLength(0);
            int width = inputImage.GetLength(1);
            int lengthF = filter.GetLength(0);
            int widthF = filter.GetLength(1);
            if (lengthF % 2 == 0 || widthF == 0)
            {
                throw new Exception("can not place pixel in the middle of filter");
            }

            for (int i = -(lengthF - 1) / 2; i < lengthF - (lengthF - 1) / 2; i++)
            {
                for (int j = -(widthF - 1) / 2; j < widthF - (widthF - 1) / 2; j++)
                {
                    if (0 <= u - i && length > u - i && 0 <= v - j && width > v - j)
                    {
                        r += inputImage[u - i, v - j] * filter[i + (lengthF - 1) / 2, j + (widthF - 1) / 2];
                    }
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
            List<byte> retImg = new List<byte>(size * size);
            int range = (size - 1) / 2;
            for (int i = x - range; i < x + range; i++)
            {
                if (i < 0) continue;
                if (i > inputImage.GetLength(0)) break;
                for (int j = y - range; j < y + range; j++)
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
                if (kernel.Length % 2 == 0) median = (kernel[h] + kernel[h - 1]) / 2;
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

        private int clamp(int i)
        {
            return i > 255 ? 255 : i;
        }

        /*
         * edgeMagnitude: calculate the image derivative of an input image and a provided edge kernel
         * input:   inputImage          single-channel (byte) image
         *          horizontalKernel    horizontal edge kernel
         *          verticalKernel      vertical edge kernel
         * output:                      single-channel (byte) image
         */
        private byte[,] edgeMagnitude(byte[,] inputImage, sbyte[,] horizontalKernel, sbyte[,] verticalKernel,
            sbyte[,] revHK, sbyte[,] revVK)
        {
            // create temporary grayscale image
            GrayScaleImage tempImage = new GrayScaleImage(new byte[inputImage.GetLength(0), inputImage.GetLength(1)]);

            float[,] fhk = map2D(horizontalKernel, b => (float)0.125 * b);
            float[,] fvk = map2D(verticalKernel, b => (float)0.125 * b);
            float[,] rHK = map2D(revHK, b => (float)0.125 * b);
            float[,] rVK = map2D(revVK, b => (float)0.125 * b);

            byte[,] h = convolveImage(inputImage, fhk);
            byte[,] v = convolveImage(inputImage, fvk);
            byte[,] rh = convolveImage(inputImage, rHK);
            byte[,] rv = convolveImage(inputImage, rVK);

            tempImage.ApplyIndexed((p, i, j) => (int)Math.Max(Math.Sqrt((h[i, j] * h[i, j]) + (v[i, j] * v[i, j])),
                Math.Sqrt((rh[i, j] * rh[i, j]) + (rv[i, j] * rv[i, j]))));
            return adjustContrast(tempImage.ToByteArray());
        }


        /*
         * thresholdImage: threshold a grayscale image
         * input:   inputImage          single-channel (byte) image
         * output:                      single-channel (byte) image with on/off values
         */
        private static byte[,] thresholdImage(byte[,] inputImage, byte threshold)
        {
            // create temporary grayscale image
            GrayScaleImage tempImage = new GrayScaleImage(inputImage);

            tempImage.Apply((p => p > threshold ? byte.MaxValue: byte.MinValue));

            return tempImage.ToByteArray();
        }
        /*
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
        */


        // ====================================================================
        // ============= YOUR FUNCTIONS FOR ASSIGNMENT 2 GO HERE ==============
        // ====================================================================

        public abstract class Imager<TA>
        {
            protected TA[,] Ar;
            public HashSet<(int u, int v)> Cps { get; protected set;} //coordinate points
            public (int x, int y) offset = (0,0);

            public virtual TA this[int u, int v]
            {
                get => Ar[u, v];
                set => Ar[u, v] = value;
            }

            public int GetLength(int d)
            {
                return Ar.GetLength(d);
            }

            public void Apply(Func<TA, TA> op)
            {
                Ar = map2D(Ar, op);
            }

            public void ApplyIndexed(Func<TA, int, int, TA> op)
            {
                Ar = map2dIndexed(Ar, op);
            }

            public static TA[,] BinaryOp(Imager<TA> a, Imager<TA> b, Func<TA, TA, TA> op)
            {
                if (b.GetLength(0) != a.Ar.GetLength(0) && b.GetLength(1) != a.Ar.GetLength(1))
                {
                    MessageBox.Show("Error in image dimensions (have to be the same)");
                    throw new Exception("Error in image dimensions (have to be the same)");
                }

                TA[,] oar = map2dIndexed(a.Ar, ((p1, i, j) => op(p1, b[i, j])));
                return oar;
            }

            //coordinate pairs
            public void UpdateCps()
            {
                Cps = new HashSet<(int u, int v)>(EnumerableRange2D(Ar.GetLength(0), Ar.GetLength(1))).ToHashSet();
            }

            public void TranslateImage(int x, int y)
            {
                offset = (x, y);
                Cps = Cps.Select(p => (p.u + x, p.v + y)).ToHashSet();
            }

            public bool InImage(int x, int y)
            {
                x -= offset.x;
                y -= offset.y;
                return x >= 0 && x < Ar.GetLength(0) && y >= 0 && y < Ar.GetLength(1);
            }

            public virtual byte[,] ToByteArray()
            {
                return map2D(Ar, p => (byte)Convert.ChangeType(p, typeof(byte)));
            }
        }

        public class BinaryImage : Imager<bool>
        {
            public HashSet<(int u, int v)> Qi;

            public BinaryImage(byte[,] image)
            {
                Ar = map2D(image, p => Convert.ToBoolean(p));
                UpdateCps();
                Qi = new HashSet<(int, int)>(Cps.Where(p => this[p.u, p.v]));
            }

            public BinaryImage(int[,] image)
            {
                Ar = map2D(image, p => Convert.ToBoolean(p));
                UpdateCps();
                Qi = new HashSet<(int, int)>(Cps.Where(p => this[p.u, p.v]));
            }

            public BinaryImage(bool[,] image)
            {
                Ar = image;
                UpdateCps();
                Qi = new HashSet<(int, int)>(Cps.Where(p => this[p.u, p.v]));
            }

            public BinaryImage(HashSet<(int u, int v)> ps, int h, int v)
            {
                Ar = new bool[h, v];
                foreach (var p in ps)
                {
                    Ar[p.u, p.v] = true;
                }

                UpdateCps();
                Qi = ps;
            }

            public GrayScaleImage HoughTransform() => HoughTransformer(Qi,map2D(Ar,v => v?1:0));

            public GrayScaleImage houghTransformAngleLimits(int minAngle, int maxAngle) =>
                houghTransformerAngleLimits(Qi, map2D(Ar, v => v ? 1 : 0), minAngle, maxAngle);
            
            public  GrayScaleImage peakFind() => peakFinding(Qi,map2D(Ar,v => v?1:0),peakThreshold);

            public List<lineSegment> houghLineDetection(int minL,int maxG)
            {
                var g = peakFind();
                var w = g.notZero();
                return houghLineDetectionG(pair => Qi.Where(p => polarCalc(p,pair.th) == pair.r).ToArray()
                    ,w,minL,maxG);
            }

            public byte[,] visualizeHoughLineSegments()
            {
                List<lineSegment> ll = houghLineDetection(minL,maxG);
                byte[,] b = new byte[GetLength(0), GetLength(1)];
                foreach (var l in ll) { l.drawLine((x, y) => b[x,y] = byte.MaxValue); }
                return b;
            }

            public static BinaryImage operator &(BinaryImage a, BinaryImage b)
            {
                return new BinaryImage(BinaryOp(a, b, ((p1, p2) => p1 && p2)));
            }

            public static BinaryImage operator |(BinaryImage a, BinaryImage b)
            {
                return new BinaryImage(BinaryOp(a, b, ((p1, p2) => p1 || p2)));
            }

            public static bool operator false(BinaryImage a) => !Foldl2D(((p1, p2) => p2 || p1), false, a.Ar);
            public static bool operator true(BinaryImage a) => Foldl2D(((p1, p2) => p2 && p1), true, a.Ar);

            public static BinaryImage operator +(BinaryImage i, StructuringElement h) //dilation
            {
                return new BinaryImage((from p in i.Qi from q in h.Qh select (p.u + q.u, p.v + q.v))
                    .Where((x) => i.InImage(x.Item1, x.Item2)).ToHashSet()
                    , i.GetLength(0), i.GetLength(1));
            }

            public static BinaryImage operator -(BinaryImage i, StructuringElement h) //erosion
            {
                HashSet<(int, int)> T((int u, int v) p) => h.Qh.Select(q => (q.u + p.u, q.v + p.v))
                    .Where((x) => i.InImage(x.Item1, x.Item2)).ToHashSet();

                return new BinaryImage((from p in i.Qi where (T(p).IsSubsetOf(i.Qi)) select p)
                    .Where((x) => i.InImage(x.Item1, x.Item2)).ToHashSet()
                    , i.GetLength(0), i.GetLength(1));
            }

            public static BinaryImage operator *(BinaryImage i, StructuringElement h) => i - h + h; //opening
            public static BinaryImage operator /(BinaryImage i, StructuringElement h) => i + h - h; //closing

            public static BinaryImage operator ~(BinaryImage i)
            {
                return new BinaryImage(i.Cps.Except(i.Qi).ToHashSet(), i.GetLength(0), i.GetLength(1));
            }

            public BinaryImage traceBoundary()
            {
                StructuringElement h = new StructuringElement(ElementShape.Star, 3);
                return new BinaryImage(Qi.Intersect((~(this - h)).Qi).ToHashSet(), this.GetLength(0),
                    this.GetLength(1));
            }

            public override byte[,] ToByteArray()
            {
                return map2D(Ar, p => p ? byte.MaxValue : byte.MinValue);
            }
        }

        public class GrayScaleImage : Imager<int>
        {
            public override int this[int u, int v]
            {
                get => Ar[u-offset.x, v-offset.y];
                set => Ar[u-offset.x, v-offset.y] = value;
            }
            
            public GrayScaleImage(byte[,] image)
            {
                Ar = map2D(image, p => (int)p);
                UpdateCps();
            }

            public GrayScaleImage(int[,] image)
            {
                Ar = image;
                UpdateCps();
            }

            public void ApplyThreshold(int t)
            {
                ApplyIndexed((p, i, j) => Ar[i, j] > t ? int.MaxValue : 0);
            }

            public GrayScaleImage HoughTransform() => HoughTransformer(notZero(),Ar);
            
            public GrayScaleImage houghTransformAngleLimits(int minAngle, int maxAngle) =>
                houghTransformerAngleLimits(notZero(), Ar, minAngle, maxAngle);
            
            public  GrayScaleImage peakFind() => peakFinding(notZero(),Ar,peakThreshold);

            public void applyStencilIndexed(int sSize, Func<int[,], int, int, int> f)
            {
                int[,] tempImage = new int[Ar.GetLength(0), Ar.GetLength(1)];
                int x = (sSize - 1) / 2;

                for (int u = 0; u < GetLength(0); u++)
                {
                    for (int v = 0; v < GetLength(1); v++)
                    {
                        int[,] stencil = new int[sSize, sSize];
                        for (int i = -1; i < x; i++)
                        {
                            for (int j = -1; j < x; j++)
                            {
                                if (u + i < 0 || u + i > GetLength(0) || v + j < 0 ||
                                    v + j > GetLength(1)) break;
                                stencil[i + x, j + x] = Ar[u + i, j + v];
                            }
                        }

                        tempImage[u, v] = f(stencil, u+offset.x, v+offset.y);
                    }
                }
                Ar = tempImage;
            }

            public List<lineSegment> houghLineDetection(int mit,int minL,int maxG)
            {
                var g = peakFind();
                var w = g.notZero();
                return houghLineDetectionG(pair => Cps.Where(p => polarCalc(p,pair.th) == pair.r && Ar[p.Item1,p.Item2] >= mit).ToArray()
                    ,w,minL,maxG);
            }

            public byte[,] visualizeHoughLineSegments()
            {
                List<lineSegment> ll = houghLineDetection(mit, minL ,maxG);
                byte[,] b = new byte[GetLength(0), GetLength(1)];
                foreach (var l in ll) { l.drawLine((x, y) => b[x,y] = byte.MaxValue); }
                return b;
            }

            public HashSet<(int, int)> notZero() => Cps.Where(p => this[p.u, p.v] != 0).ToHashSet();

            public int getMax()
            {
                return Foldl2D((v, a) => v > a ? v : a, 0, Ar);
            }

            public byte getMin(byte[,] img)
            {
                return Foldl2D((v, a) => v < a ? v : a, byte.MaxValue, img);
            }

            public (int, Dictionary<int, int>) countValues()
            {
                Dictionary<int, int> Count(int v, Dictionary<int, int> counter)
                {
                    if (counter.ContainsKey(v))
                    {
                        counter[v]++;
                    }
                    else
                    {
                        counter.Add(v, 1);
                    }

                    return counter;
                }

                var d = Foldl2D(Count
                    , new Dictionary<int, int>()
                    , Ar);
                return (d.Count, d);
            }

            public static GrayScaleImage operator +(GrayScaleImage i, StructuringElement h) //dilation
            {
                var ni = new GrayScaleImage(new int[i.Ar.GetLength(0), i.Ar.GetLength(1)]);
                ni.TranslateImage(i.offset.x,i.offset.y);
                foreach (var p in i.Cps)
                {
                    ni[p.u, p.v] = h.Qh.Max(q =>
                        i.InImage(p.u + q.u, p.v + q.v)?
                            i[p.u+q.u,p.v+q.v] > 0?
                            i[p.u + q.u, p.v + q.v] + h[q.u, q.v]:0:0);
                }
                return ni;
            }

            public static GrayScaleImage operator -(GrayScaleImage i, StructuringElement h) //dilation
            {
                var ni = new GrayScaleImage(new int[i.Ar.GetLength(0), i.Ar.GetLength(1)]);
                ni.TranslateImage(i.offset.x,i.offset.y);
                foreach (var p in i.Cps)
                {
                    ni[p.u, p.v] = h.Qh.Min(q =>
                        i.InImage(p.u + q.u, p.v + q.v) ? 
                            i[p.u+q.u,p.v+q.v] > 0?
                                i[p.u + q.u, p.v + q.v] + h[q.u, q.v] 
                                :0:int.MaxValue);
                }
                return ni;
            }

            public static GrayScaleImage operator *(GrayScaleImage i, StructuringElement h) => i - h + h; //opening
            public static GrayScaleImage operator /(GrayScaleImage i, StructuringElement h) => i + h - h; //closing
            
            public override byte[,] ToByteArray()
            {
                return map2D(Ar, p => p < 256 ? (byte)p : Byte.MaxValue);
            }
        }

        public class StructuringElement : Imager<int>
        {
            public HashSet<(int u, int v)> Qh;

            private (int x, int y) Offset
            {
                get => offset;
                set => offset = (-(value.x - 1) / 2, -(value.y - 1) / 2);
            }

            public StructuringElement(ElementShape shape, int size)
            {
                if (size % 2 == 0)
                {
                    MessageBox.Show("Element Size is even, please enter an odd element size");
                    throw new Exception(">:(");
                }

                Ar = shape == ElementShape.Square
                    ? new[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } }
                    : new[,] { { -1, 1, -1 }, { 1, 1, 1 }, { -1, 1, -1 } };
                UpdateCps();
                Offset = (3, 3);
                TranslateImage(offset.x, offset.y);
                Qh = new HashSet<(int, int)>(Cps.Where(p => this[p.u, p.v] == 1));
                int x = (size - 3) / 2;
                if (x > 0)
                {
                    var s = Enumerable.Range(0, x).Aggregate(this, (se, _) => se + new StructuringElement(shape, 3));
                    Qh = s.Qh;
                    Cps = s.Cps;
                    Ar = s.Ar;
                }
            }

            public StructuringElement(HashSet<(int u, int v)> ps, int h, int v)
            {
                Ar = new int[h, v];
                Offset = (h, v);
                foreach (var p in ps)
                {
                    this[p.u, p.v] = 1;
                }

                UpdateCps();
                Qh = ps;
            }

            public override int this[int u, int v]
            {
                get => base[u - offset.x, v - offset.y];
                set => base[u - offset.x, v - offset.y] = value;
            }

            public static StructuringElement operator +(StructuringElement h1, StructuringElement h2) //dilation
            {
                var h3 = (from p in h1.Qh from q in h2.Qh select (p.u + q.u, p.v + q.v))
                    .Where((x) => h1.InImage(x.Item1, x.Item2)).ToHashSet();
                return new StructuringElement(h3, h3.Max(p => Math.Abs(p.Item1) * 2 + 1),
                    h3.Max(p => Math.Abs(p.Item2) * 2 + 1));
            }
        }

        private StructuringElement createStructuringElement(ElementShape shape, int size, bool binary)
        {
            return new StructuringElement(shape, size);
        }

        /*private int[,] dilateR(int[,] structuringElement, bool binary, int x, ElementShape shape)
        {
            if (x <= 0)
            {
                return structuringElement;
            }

            int[,] newInput = putInCenter(structuringElement.GetLength(0) + 2, structuringElement.GetLength(1) + 2,
                structuringElement);
            return dilateR(dilateImage(newInput, this.structuringElement(shape, 3, binary), binary), binary, x-1, shape);
        }

        private int[,] putInCenter(int nw, int nh, int[,] shape)
        {
            int[,] tempImg = setZeros(nw, nh);
            for (int i = 0; i < shape.GetLength(0); i++)
            {
                for (int j = 0; j < shape.GetLength(1); j++)
                {
                    tempImg[i + 1, j + 1] = shape[i, j];
                }
            }
            return tempImg;
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
                        if (inputImage[i, j] > 127)
                        {
                            for (int k = -x; k <= x; k++)
                            {
                                for (int l = -x; l <= x; l++)
                                {
                                    if (i + k >= 0 && i + k < inputImage.GetLength(0) && j + l >= 0 &&
                                        j + l < inputImage.GetLength(1) && structuralElement[k+x, l+x] == 255)
                                    {
                                        outputImage[i + k, j + l] = 255;
                                    }
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
                                if (i + k >= 0 && i + k < inputImage.GetLength(0) && j + l >= 0 &&
                                    j + l < inputImage.GetLength(1) && (structuralElement[k + x, l + x] != -1))
                                {
                                    temp[k+x, l+x] = inputImage[i + k, j + l] + structuralElement[k+x, l+x];
                                    
                                }
                            }
                        }
                        //outputImage[i, j] = (int)getMax(PointOperationImage(temp, (i1, I, J) => (byte)i1));
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
                                if (i + k >= 0 && i + k < inputImage.GetLength(0) && j + l >= 0 &&
                                    j + l < inputImage.GetLength(1))
                                {
                                    check = ((inputImage[i + k, j + l] == 255 && structuralElement[k+x, l+x] == 255) || (inputImage[i+k, j+l] == 255 && structuralElement[k+x,l+x] == 0) || (inputImage[i+k, j+l] == 0 && structuralElement[k+x,l+x] == 0)) && check;
                                }
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
                                if (i + k >= 0 && i + k < inputImage.GetLength(0) && j + l >= 0 &&
                                    j + l < inputImage.GetLength(1) && (structuralElement[k + x, l + x] != -1))
                                {
                                    int m = structuralElement[k + x, l + x];
                                    int n = inputImage[i + k, j + l];
                                    int val = n - m;
                                    temp[k + x, l + x] = val < 0 ? 0 : inputImage[i + k, j + l] - structuralElement[k + x, l + x];
                                }
                            }
                        }
                        outputImage[i, j] = (int)//getMin(PointOperationImage(temp, (i1, I, J) => (byte)i1));
                    }
                }
            }
            return outputImage;
        }

        private int[,] openImage(int[,] inputImage, int[,] structuralElement, bool binary)
        {
            int[,] workingimage = inputImage;
            if (binary)
            {
                workingimage = byteArrayToIntArray(thresholdImage(intArrayToByteArray(workingimage), threshold));
            }
            int[,] erodedImage = erodeImage(workingimage, structuralElement, binary);
            int[,] dilatedImage = dilateImage(erodedImage, structuralElement, binary);
            return dilatedImage;
        }

        private int[,] closeImage(int[,] inputImage, int[,] structuralElement, bool binary)
        {
            int[,] workingimage = inputImage;
            if (binary)
            {
                workingimage = byteArrayToIntArray(thresholdImage(intArrayToByteArray(workingimage), threshold));
            }
            int[,] dilatedImage = dilateImage(workingimage, structuralElement, binary);
            int[,] erodedImage = erodeImage(dilatedImage, structuralElement, binary);
            return erodedImage;
        }
        
        */

        // ====================================================================
        // ============= YOUR FUNCTIONS FOR ASSIGNMENT 3 GO HERE ==============
        // ====================================================================
        public static double peakThreshold = 0.90;
        public static int maxG = 10;
        public static int minL = 50;
        public static int mit = 20;
        
        public static int polarCalc((int x,int y) p, double tetha) => (int)(p.x * Math.Cos(tetha) + p.y * Math.Sin(tetha));
        public static int pythagorar2D((int x,int y)p1,(int x,int y)p2) => (int)Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2));
        
        public static GrayScaleImage HoughTransformer(HashSet<(int x, int y)> edgePixels,int[,] Ar,int minAngle=0,int maxAngle=360)
        {
            Dictionary<(int th, int r), int> accumulator = new Dictionary<(int, int), int>();
            accumulator = edgePixels.Aggregate(accumulator, (d, p) => HoughAdd(d,(p.x,p.y,Ar[p.x,p.y]),minAngle,maxAngle));
            int w = accumulator.Keys.Max(k => k.th) + 1;
            int h = accumulator.Keys.Max(k => k.r) + 1;
            int offset = accumulator.Keys.Min(k => k.r);
            int[,] nAr = new int[w, h - offset];
            foreach (var pair in accumulator)
            {
                nAr[pair.Key.th, pair.Key.r - offset] = pair.Value;
            }
            var hr = new GrayScaleImage(nAr);
            hr.TranslateImage(0,offset);
            return hr;
        }

        private static Dictionary<(int th, int r), int> HoughAdd(Dictionary<(int th, int r), int> aar, (int x, int y,int v) p,int minAngle,int maxAngle)
        {
            var e = Enumerable.Range((int)(minAngle/360f*500),(int)(maxAngle/360f*500));

            Dictionary<(int, int), int> Count(Dictionary<(int x, int y), int> counter, int th)
            {
                double thp = 2 * Math.PI * (th / 500f);
                int r = polarCalc((p.x,p.y), thp);
                void add(int tha,int ra){if (counter.ContainsKey((tha, ra)))
                {
                    if (counter[(tha, ra)] < 256)
                    {
                        counter[(tha, ra)] += p.v;
                    }
                }
                else
                {
                    counter.Add((tha, ra),p.v);
                }}
                add(th,r-1);
                add(th,r);
                add(th,r+1);
                return counter;
            }

            aar = e.Aggregate(aar, Count);
            return aar;
        }
        
        //option A
        public static GrayScaleImage peakFinding(HashSet<(int x, int y)> edgePixels,int[,]Ar,double threshold)
        {
            GrayScaleImage ht = HoughTransformer(edgePixels,Ar);
            int thresholdp = (int)(threshold * ht.getMax());
            ht.Apply(v => v>=thresholdp?v:0);
            StructuringElement e = new StructuringElement(ElementShape.Square, 35);
            GrayScaleImage cht = Enumerable.Range(0,1).Aggregate(ht,(v, _) => v/e);
            return cht;
        }
        
        //option B
        private static GrayScaleImage peakFindingB(HashSet<(int x, int y)> edgePixels,int[,]Ar,double threshold)
        {
            GrayScaleImage inputImage = HoughTransformer(edgePixels, Ar);
            //we implement option B
            int f(int[,] s, int x, int y)
            {
                int max = Foldl2D(((a, i) => (a > i ? a : i)), 0, s);
                int val = inputImage[x, y];
                return max > val ? max : val;
            }
            inputImage.applyStencilIndexed(3, f);
            int thresholdp = (int)(threshold * inputImage.getMax());
            inputImage.ApplyThreshold(thresholdp);
            return inputImage;
        }

        public struct lineSegment
        {
            public lineSegment((int, int) start, (int, int) end)
            {
                Start = start;
                End = end;
            }

            public (int x, int y) Start { get; }
            public (int x, int y) End { get; }

            public int GetLength() => pythagorar2D(Start, End);

            public delegate void DrawPixel(int x, int y);

            public void drawLine(DrawPixel draw)
            {

                int dx = End.x - Start.x, dy = End.y - Start.y;
                int nx = Math.Abs(dx), ny = Math.Abs(dy);
                int sign_x = dx > 0 ? 1 : -1, sign_y = dy > 0 ? 1 : -1;
                var p = Start;
                for (int ix = 0, iy = 0; ix < nx || iy < ny;)
                {
                    if ((0.5 + ix) / nx < (0.5 + iy) / ny)
                    {
                        // next step is horizontal
                        p.x += sign_x;
                        ix++;
                    }
                    else
                    {
                        // next step is vertical
                        p.y += sign_y;
                        iy++;
                    }

                    draw(p.x, p.y);
                }
            }
        }

        public static List<lineSegment> houghLineDetectionG(Func<(double th,int r),(int,int)[]> f,HashSet<(int,int)>ht, int minL, int maxG)
        {
            List<lineSegment> lineSegments = new List<lineSegment>();
            List<lineSegment> createLineSegmentB(List<lineSegment> ll,(int th,int r) pair)
            {
                var thp = (2 * Math.PI * (pair.th / 500f),pair.r);
                var valid = f(thp);
                if (valid.Length > 0)
                {
                    (int x, int y) start = valid.First();
                    (int x, int y) end = valid.First();
                    foreach (var vp in valid.Skip(1))
                    {
                        if (pythagorar2D(end, vp) > maxG) //gap between pixels to big
                        {
                            if (pythagorar2D(start, end) >= minL){ll.Add(new lineSegment(start, end));}
                            start = vp;
                        }
                        end = vp;
                    }
                    if (pythagorar2D(start, end) >= minL){ll.Add(new lineSegment(start, end));}
                }

                return ll;
            }
            return ht.Aggregate(lineSegments,createLineSegmentB).ToList();
        }

        public static GrayScaleImage houghTransformerAngleLimits(HashSet<(int x, int y)> edgePixels, int[,] Ar,
            int minAngle, int maxAngle) => HoughTransformer(edgePixels, Ar, minAngle, maxAngle);
    }
}