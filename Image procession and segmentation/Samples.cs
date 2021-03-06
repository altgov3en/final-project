﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Image_procession_and_segmentation
{
    class Samples
    {
        private Histogram imageHistogram;
        private Bitmap image;
        Clusters[] clustersForEstimation;
        Histogram[] histograms;
        double[] samplesDifferences;


        private int samplingRate; //how many histogram samples will be made

        private Tuple<double[], double[]> emResultMeanAndSdev;

        double[][] sDeviationForKmaxClusters;
        double[][] meanResultsForKmaxClusters;

        public Samples(Histogram h, Bitmap i, int k)
        {
            this.imageHistogram = h;
            this.image = i;
            this.samplingRate = k;
            this.clustersForEstimation = new Clusters[this.samplingRate];
            this.histograms = new Histogram[this.samplingRate];
            this.samplesDifferences = new double[this.samplingRate];
            this.sDeviationForKmaxClusters = new double[this.samplingRate][];
            this.meanResultsForKmaxClusters = new double[this.samplingRate][];
            this.imageHistogram.CalculateCumulativeSumOfHistogramPeaks();           
        }

        public int makeEstimationOfClusterNumber()
        {
            

            for (int i = 0; i < this.samplingRate; i++)
            {   // Create kMax samples of original histogram
                // Every sample will contain 50 randomly picked colors (0-255) from original histogram

                this.histograms[i] = new Histogram(image);
                this.histograms[i].GetHistogramSamples();

                this.clustersForEstimation[i] = new Clusters(i + 2, this.image.Height,
                                                                    this.image.Width,
                                                                    this.histograms[i],
                                                                    this.image, i, true);

                this.emResultMeanAndSdev = clustersForEstimation[i].estimateClusterNumber();
                this.meanResultsForKmaxClusters[i] = this.emResultMeanAndSdev.Item1;
                this.sDeviationForKmaxClusters[i]  = this.emResultMeanAndSdev.Item2;

                // For all color it will define and count to which cluster the color is belong.
                this.clustersForEstimation[i].AssignColorsToCluster(this.emResultMeanAndSdev.Item1);
                this.clustersForEstimation[i].CalculateCDF(this.emResultMeanAndSdev.Item1);
                this.samplesDifferences[i] = this.clustersForEstimation[i].histogram.sumOfHistogramDifference;
     
            }
                double minValue = this.samplesDifferences.Min();
                return (this.samplesDifferences.ToList().IndexOf(minValue) + 2);
            
        }

    }
}
