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
using GPStudio.Shared;

namespace GPStudio.Server
{
	/// <summary>
	/// Base class used for all terminal node types, derived from GPNode
	/// and implements ICloneable. 
	/// </summary>
	public abstract class GPNodeTerminal : GPNode, ICloneable
	{
		public abstract String ToValueTypeString();
		
		/// <summary>
		/// ICloneable interface implementation
		/// </summary>
		/// <returns></returns>
		public override Object Clone()
		{
			return this.MemberwiseClone();
		}
		
		/// <summary>
		/// Static method to randomly create a terminal node
		/// TODO: Need to override this method for each type, all that if logic is stupid!
		/// </summary>
		/// <param name="bADF"></param>
		/// <param name="ADFArgCount"></param>
		/// <param name="bADL"></param>
		/// <param name="ADLArgCount"></param>
		/// <param name="bADR"></param>
		/// <param name="ADRArgCount"></param>
		/// <param name="TerminalSet"></param>
		/// <param name="IntegerMin"></param>
		/// <param name="IntegerMax"></param>
		/// <returns></returns>
		public static GPNodeTerminal Initialize(
			bool bADF, short ADFArgCount, 
			bool bADL,short ADLArgCount,
			bool bADR,short ADRArgCount,
			List<GPEnums.TerminalType> TerminalSet, int IntegerMin, int IntegerMax)
		{
			short Random = 0;
			GPNodeTerminal node;

			//
			// Go through the terminal set and count up the number of user
			// define terminal types.
			short UserDefined = 0;
			for (short Terminal = 0; Terminal < TerminalSet.Count; Terminal++)
			{
				if (TerminalSet[Terminal] == GPEnums.TerminalType.UserDefined)
				{
					UserDefined++;
				}
			}

			//
			// If we have an ADF, then see if we randomly select the terminal to
			// be a function parameter or a random constant.  The (TerminalSet.Count-UserDefined)
			// is to get down to just the random constant options.
			if (bADF)
			{
				Random = (short)GPUtilities.rngNextInt(ADFArgCount + (TerminalSet.Count-UserDefined));
				if (Random < ADFArgCount)
				{
					node = new GPNodeTerminalADFParam(Random);
					return node;
				}
				//
				// Update the random selection to be one of the random constant options
				Random -= ADFArgCount;
				Random += UserDefined;
			}

			//
			// If we have an ADL, do the same kind of thing as above for ADFs
			if (bADL)
			{
				Random = (short)GPUtilities.rngNextInt(ADLArgCount + (TerminalSet.Count-UserDefined));
				if (Random < ADLArgCount)
				{
					node = new GPNodeTerminalADLParam(Random);
					return node;
				}
				//
				// Update the random selection to be one of the random constant options
				Random -= ADLArgCount;
				Random += UserDefined;
			}

			//
			// Same story for ADRs
			if (bADR)
			{
				Random = (short)GPUtilities.rngNextInt(ADRArgCount + (TerminalSet.Count - UserDefined));
				if (Random < ADRArgCount)
				{
					node = new GPNodeTerminalADRParam(Random);
					return node;
				}
				//
				// Update the random selection to be one of the random constant options
				Random -= ADRArgCount;
				Random += UserDefined;
			}

			//
			// If an RPB terminal, select from anything in the terminal set
			if (!bADF && !bADL && !bADR)
			{
				Random = (short)GPUtilities.rngNextInt(TerminalSet.Count);
			}

			//
			// It is a user defined terminal
			if (Random < UserDefined)
			{
				return new GPNodeTerminalUserDefined(Random);
			}
			//
			// It is either an integer or double terminal
			if (TerminalSet[Random] == GPEnums.TerminalType.RandomDouble)
			{
				return new GPNodeTerminalDouble(GPUtilities.rngNextDouble());

			}
			else
			{
				return new GPNodeTerminalInteger(GPUtilities.rngNextInt(IntegerMin, IntegerMax));
			}
		}

		/// <summary>
		/// Terminals require no editing
		/// </summary>
		public override GPNode Edit(GPProgram tree, GPProgramBranch execBranch) { return this; }
	}
}
