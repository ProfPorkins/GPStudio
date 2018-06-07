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
using GPStudio.Shared;

/// <summary>
/// This namespace provides a description for all the interfaces exposed
/// by the GPServer.  No implementations exist in this namespace, only
/// the interface definitions.
/// </summary>
namespace GPStudio.Interfaces
{
	/// <summary>
	/// Factory interface that allows clients to obtain unique interfaces for
	/// each of the interfaces into the GPServer service.
	/// </summary>
	public interface IGPServer
	{
		IGPModeler Modeler
		{
			get;
		}

		IGPCompiler Compiler
		{
			get;
		}

		IGPCustomFitness CustomFitness
		{
			get;
		}

		IGPFunctionSet FunctionSet
		{
			get;
		}

		IGPProgram Program
		{
			get;
		}

		IGPLanguageWriter LanguageWriter
		{
			get;
		}

		int Validate();
	}

	/// <summary>
	/// Interface that allows clients to generate models.  The client controls
	/// the modeling, but this interface describes the properties and operations
	/// available for creating models.
	/// </summary>
	public interface IGPModeler
	{
		IGPFunctionSet FunctionSet
		{
			set;
			get;
		}

		GPTrainingData Training
		{
			set;
		}

		String FitnessFunction { set; }

		GPModelingProfile Profile
		{
			set;
		}

		void AddADF(byte ParamCount);
		void AddADL(byte ParamCount);
		void AddADR(byte ParamCount);


		void InitializePopulation(int PopulationSize);
		void EvaluateFitness(int Generation);
		void ComputeNextGeneration(int Generation);

		void AddModeler(IGPModeler iServer);
		void DistributePopulation();
		void AddSeedProgram(byte[] xmlBytes);

		void Abort();
		void ForceCleanup();

		String BestProgram { get; }
		byte[] SelectProgramTournament { get; }
		void GetBestProgramStats(out double Fitness, out int Hits, out int Complexity);
		void GetPopulationStats(out double FitnessMax, out double FitnessAve, out int ComplexityMin, out int ComplexityMax, out int ComplexityAve);
	}

	/// <summary>
	/// Interface that provides the ability to manage a set of user defined functions
	/// </summary>
	public interface IGPFunctionSet
	{
		void Clear();
		bool AddFunction(String Name, short Arity, bool TerminalParameters, String UserCode);
		bool UseMemory
		{
			set;
		}

		bool UseInputHistory
		{
			get;
		}

		Object this[string Function]
		{
			get;
		}

		Object this[int Function]
		{
			get;
		}

		int Count
		{
			get;
		}

		int IndexOfKey(string Key);
	}

	/// <summary>
	/// Interface that provides the ability to write and validate user and fitness functions
	/// </summary>
	public interface IGPCompiler
	{
		String WriteUserFunction(String Name,short Arity,String UserCode);
		String WriteFitnessClass(String UserCode);
		bool ValidateUserFunction(String Name, String FunctionCode, out String[] Errors);
		bool ValidateFitnessClass(String FitnessCode, out String[] Errors);
	}

	/// <summary>
	/// Interface that allows a remote client to control a single GPProgram
	/// object at a server.
	/// </summary>
	public interface IGPProgram
	{
		IGPFunctionSet FunctionSet
		{
			set;
		}

		GPTrainingData Training
		{
			set;
		}

		bool ProgramFromXML(String ProgramXML);

		double[] ComputeBatch();
		double[] ComputeBatchTS(int InputDimension, int PredictionDistance);
	}

	/// <summary>
	/// Interface that allows a client to obtain the fitness of a program
	/// </summary>
	public interface IGPCustomFitness
	{
		String FitnessFunction
		{
			set;
		}

		GPTrainingData Training
		{
			set;
		}

		double[] Predictions
		{
			set;
		}

		double Compute();
	}

	/// <summary>
	/// Interface that provides the ability to write a program using any of
	/// the specified languages.
	/// </summary>
	public interface IGPLanguageWriter
	{
		void Clear();
		bool AddFunction(String Name, short Arity, String UserCode);

		String WriteC(IGPProgram Program, bool TimeSeries);
		String WriteCPP(IGPProgram Program, bool TimeSeries);
		String WriteCSharp(IGPProgram Program, bool TimeSeries);
		String WriteJava(IGPProgram Program, bool TimeSeries);
		String WriteVBNet(IGPProgram Program, bool TimeSeries);
		String WriteFortran(IGPProgram Program, bool TimeSeries);
		String WriteXML(IGPProgram Program, bool TimeSeries);
	}
}
