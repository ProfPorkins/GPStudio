/*
Copyright (c) 2018 James Dean Mathias

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace GPStudio.Server
{
	/// <summary>
	/// A simple abstract class defintion from which all custom fitness classes
	/// are derived.  The purpose of this class is to give the GPFitness class
	/// a known class and method to call when computing the fitness for some
	/// program.
	/// 
	/// There are NO concrete fitness function class implementations in the GPServer
	/// code.  All fitness functions are transmitted to the server at runtime
	/// from the client.
	/// </summary>
	public abstract class GPFitnessCustomBase
	{
		public GPFitnessCustomBase() { }

		public abstract double ComputeFitness(List<List<double>> InputHistory,double[] Predictions, double[] Training, double TrainingAverage, double Tolerance);
	}
}
