﻿/*<copyright>
Mo+ Solution Builder is a model oriented programming language and IDE, used for building models and generating code and other documents in a model driven development process.

Copyright (C) 2013 Dave Clemmer, Intelligent Coding Solutions, LLC

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
</copyright>*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;
using Irony.Interpreter.Ast;
using MoPlus.Interpreter.BLL.Solutions;
using MoPlus.Interpreter.BLL.Config;
using MoPlus.Interpreter.BLL.Entities;
using MoPlus.Data;
using System.Collections;
using MoPlus.Interpreter.BLL.Specifications;
using MoPlus.Interpreter.Resources;
using MoPlus.IO;

namespace MoPlus.Interpreter.BLL.Interpreter
{
	///--------------------------------------------------------------------------------
	/// <summary>This class implements necessary members for interpretation of trace statements.</summary>
	///
	/// <CreatedByUserName>DAVE\JavaDave</CreatedByUserName>
	/// <CreatedDate>9/29/2011</CreatedDate>
	/// <Status>Customized</Status>
	///--------------------------------------------------------------------------------
	public class TraceStatementNode : NonLeafGrammarNode, IStatementNode
	{
		///--------------------------------------------------------------------------------
		/// <summary>This property gets the DisplayName, for display purposes, etc.</summary>
		///--------------------------------------------------------------------------------
		public override string DisplayName
		{
			get
			{
				return LineNumber.ToString() + ": " + DisplayValues.TemplateNode_TraceStatement;
			}
		}

		public ParameterNode Text { get; set; }
		public ParameterNode FilePath { get; set; }

		///--------------------------------------------------------------------------------
		/// <summary>This method sets up the AST node and its children in the AST.</summary>
		/// 
		/// <param name="context">The parsing context.</param>
		/// <param name="treeNode">The corresponding node in the parse tree.</param>
		///--------------------------------------------------------------------------------
		public override void Init(ParsingContext context, ParseTreeNode treeNode)
		{
			try
			{
				base.Init(context, treeNode);
				foreach (ParseTreeNode node in treeNode.ChildNodes)
				{
					if (node.AstNode is ParameterNode)
					{
						if (Text == null)
						{
							Text = node.AstNode as ParameterNode;
							ChildNodes.Add(node.AstNode as ParameterNode);
						}
						else if (FilePath == null)
						{
							FilePath = node.AstNode as ParameterNode;
							ChildNodes.Add(node.AstNode as ParameterNode);
						}
					}
				}
			}
			catch (ApplicationAbortException)
			{
				throw;
			}
			catch (System.Exception ex)
			{
				ThrowASTException(ex, true);
			}
		}

		///--------------------------------------------------------------------------------
		/// <summary>Interpret this node to produce code, output, or model data..</summary>
		/// 
		/// <param name="interpreterType">The type of interpretation to perform.</param>
		/// <param name="solutionContext">The associated solution.</param>
		/// <param name="templateContext">The associated template.</param>
		/// <param name="modelContext">The associated model context.</param>
		///--------------------------------------------------------------------------------
		public void InterpretNode(InterpreterTypeCode interpreterType, Solution solutionContext, ITemplate templateContext, IDomainEnterpriseObject modelContext)
		{
			try
			{
				string parameterValue = String.Empty;
				if (Text != null)
				{
					parameterValue = Text.GetStringValue(solutionContext, templateContext, modelContext, interpreterType);
				}
				string message = ShowTraceMessage(solutionContext, templateContext, modelContext, parameterValue, false);
				if (FilePath != null)
				{
					string path = FilePath.GetStringValue(solutionContext, templateContext, modelContext, interpreterType);
					FileHelper.AppendToFile(path, message);
				}
			}
			catch (ApplicationAbortException)
			{
				throw;
			}
			catch (System.Exception ex)
			{
				LogException(solutionContext, templateContext, modelContext, ex, interpreterType);
			}
		}
	}
}
