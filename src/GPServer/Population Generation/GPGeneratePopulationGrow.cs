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
using System.Collections.Generic;
using GPStudio.Shared;

namespace GPStudio.Server
{
	/// <summary>
	/// This class generates an initial Population using the GROW method
	/// for all programs in the Population.
	/// </summary>
	public class GPGeneratePopulationGrow : GPGeneratePopulation
	{
		public GPGeneratePopulationGrow(GPModelerServer Config, short InputDimension)
			: base(Config, InputDimension)
		{
		}
		
		public override List<GPProgram> Generate(long Size)
		{
		List<GPProgram> Population = new List<GPProgram>();
		
			//
			// Create the indicated number of programs
			for (long Program=0; Program<Size; Program++)
			{
				Population.Add(m_TreeFactory.Construct(m_Config.Profile.PopulationInitialDepth, GPEnums.TreeBuild.Grow));
			}
		
		return Population;
		}

	}
}
