using System;
using System.IO;
using System.Reflection;

using NUnit.Framework;

using NDoc.Core;

public class TemplateTests : TestCase
{
	public TemplateTests(string name) : base(name) {}

	AssemblyNavigator assemblyNavigator;
	string documentation;

	protected override void SetUp()
	{
		Assembly assembly = typeof(NDoc.Test.Class1).Assembly;
		string location = assembly.Location;
		string directory = Path.GetDirectoryName(location);
		documentation = Path.Combine(directory, "NDocTest.xml");
		assemblyNavigator = new AssemblyNavigator(assembly);
	}
	
	protected override void TearDown()
	{
	}

	private string EvaluateNamespace(string templateXml, string namespaceName)
	{
		Template template = new Template();
		template.LoadXml(templateXml);
		StringWriter result = new StringWriter();
		template.EvaluateNamespace(namespaceName, assemblyNavigator, documentation, result);
		return result.ToString();
	}

	private string EvaluateType(string templateXml, string namespaceName, string typeName)
	{
		Template template = new Template();
		template.LoadXml(templateXml);
		StringWriter result = new StringWriter();
		template.EvaluateType(namespaceName, typeName, assemblyNavigator, documentation, result);
		return result.ToString();
	}

	public void TestLiteralElement()
	{
		AssertEquals("<foo />", EvaluateNamespace("<foo />", "NDoc.Test"));
	}

	public void TestLiteralElementWithTextContent()
	{
		AssertEquals("<foo>bar</foo>", EvaluateNamespace("<foo>bar</foo>", "NDoc.Test"));
	}

	public void TestLiteralAttribute()
	{
		AssertEquals("<foo bar=\"baz\" />", EvaluateNamespace("<foo bar=\"baz\" />", "NDoc.Test"));
	}

	public void TestLiteralAttributeWithAssemblyNameVariable()
	{
		AssertEquals("<foo bar=\"NDocTest\" />", EvaluateNamespace("<foo bar=\"{$assembly-name}\" />", "NDoc.Test"));
	}

	public void TestLiteralAttributeWithAssemblyNameVariableSurroundedByText()
	{
		AssertEquals("<foo bar=\"bazNDocTestquux\" />", EvaluateNamespace("<foo bar=\"baz{$assembly-name}quux\" />", "NDoc.Test"));
	}

	public void TestLiteralAttributeWithVariableWithNoDollarSign()
	{
		AssertEquals("<foo bar=\"{baz}\" />", EvaluateNamespace("<foo bar=\"{baz}\" />", "NDoc.Test"));
	}

	public void TestLiteralAttributeWithVariableWithNoCloseCurly()
	{
		AssertEquals("<foo bar=\"{$baz\" />", EvaluateNamespace("<foo bar=\"{$baz\" />", "NDoc.Test"));
	}

	public void TestLiteralAttributeWithVariableWithNoName()
	{
		AssertEquals("<foo bar=\"{$}\" />", EvaluateNamespace("<foo bar=\"{$}\" />", "NDoc.Test"));
	}

	public void TestLiteralAttributeWithVariableWithNoNameAndNoCloseCurly()
	{
		AssertEquals("<foo bar=\"{$\" />", EvaluateNamespace("<foo bar=\"{$\" />", "NDoc.Test"));
	}

	public void TestLiteralAttributeWithVariableWithUnknownName()
	{
		AssertEquals("<foo bar=\"{$baz}\" />", EvaluateNamespace("<foo bar=\"{$baz}\" />", "NDoc.Test"));
	}

	public void TestTemplate()
	{
		AssertEquals(String.Empty, EvaluateNamespace("<template />", "NDoc.Test"));
	}

	public void TestTemplateWithTextContent()
	{
		AssertEquals("foo", EvaluateNamespace("<template>foo</template>", "NDoc.Test"));
	}

	public void TestAssemblyName()
	{
		AssertEquals("NDocTest", EvaluateNamespace("<assembly-name />", "NDoc.Test"));
	}

	public void TestNamespaceName()
	{
		AssertEquals("NDoc.Test", EvaluateNamespace("<namespace-name />", "NDoc.Test"));
	}

	public void TestTypeName()
	{
		AssertEquals("Class1", EvaluateType("<type-name />", "NDoc.Test", "Class1"));
	}

	public void TestIfNamespaceContainsClasses()
	{
		AssertEquals(
			"true",
			EvaluateNamespace(
				"<if-namespace-contains-classes>true</if-namespace-contains-classes>",
				"NDoc.Test.TwoClasses"));

		AssertEquals(
			String.Empty,
			EvaluateNamespace(
				"<if-namespace-contains-classes>false</if-namespace-contains-classes>",
				"NDoc.Test.TwoInterfaces"));
	}

	public void TestIfNamespaceContainsInterfaces()
	{
		AssertEquals(
			"true",
			EvaluateNamespace(
				"<if-namespace-contains-interfaces>true</if-namespace-contains-interfaces>",
				"NDoc.Test.TwoInterfaces"));

		AssertEquals(
			String.Empty,
			EvaluateNamespace(
				"<if-namespace-contains-interfaces>false</if-namespace-contains-interfaces>",
				"NDoc.Test.TwoClasses"));
	}

	public void TestIfNamespaceContainsStructures()
	{
		AssertEquals(
			"true",
			EvaluateNamespace(
				"<if-namespace-contains-structures>true</if-namespace-contains-structures>",
				"NDoc.Test.TwoStructures"));

		AssertEquals(
			String.Empty,
			EvaluateNamespace(
				"<if-namespace-contains-structures>false</if-namespace-contains-structures>",
				"NDoc.Test.TwoClasses"));
	}

	public void TestIfNamespaceContainsDelegates()
	{
		AssertEquals(
			"true",
			EvaluateNamespace(
				"<if-namespace-contains-delegates>true</if-namespace-contains-delegates>",
				"NDoc.Test.TwoDelegates"));

		AssertEquals(
			String.Empty,
			EvaluateNamespace(
				"<if-namespace-contains-delegates>false</if-namespace-contains-delegates>",
				"NDoc.Test.TwoClasses"));
	}

	public void TestIfNamespaceContainsEnumerations()
	{
		AssertEquals(
			"true",
			EvaluateNamespace(
				"<if-namespace-contains-enumerations>true</if-namespace-contains-enumerations>",
				"NDoc.Test.TwoEnumerations"));

		AssertEquals(
			String.Empty,
			EvaluateNamespace(
				"<if-namespace-contains-enumerations>false</if-namespace-contains-enumerations>",
				"NDoc.Test.TwoClasses"));
	}

	public void TestForEachClassInNamespace()
	{
		AssertEquals(
			"Class1Class2", 
			EvaluateNamespace(
				"<for-each-class-in-namespace><type-name /></for-each-class-in-namespace>", 
				"NDoc.Test.TwoClasses"));
	}

	public void TestForEachInterfaceInNamespace()
	{
		AssertEquals(
			"Interface1Interface2", 
			EvaluateNamespace(
				"<for-each-interface-in-namespace><type-name /></for-each-interface-in-namespace>", 
				"NDoc.Test.TwoInterfaces"));
	}

	public void TestForEachStructureInNamespace()
	{
		AssertEquals(
			"Structure1Structure2", 
			EvaluateNamespace(
				"<for-each-structure-in-namespace><type-name /></for-each-structure-in-namespace>", 
				"NDoc.Test.TwoStructures"));
	}

	public void TestForEachDelegateInNamespace()
	{
		AssertEquals(
			"Delegate1Delegate2", 
			EvaluateNamespace(
				"<for-each-delegate-in-namespace><type-name /></for-each-delegate-in-namespace>", 
				"NDoc.Test.TwoDelegates"));
	}

	public void TestForEachEnumerationInNamespace()
	{
		AssertEquals(
			"Enumeration1Enumeration2", 
			EvaluateNamespace(
				"<for-each-enumeration-in-namespace><type-name /></for-each-enumeration-in-namespace>", 
				"NDoc.Test.TwoEnumerations"));
	}

	public void TestForEachOfEverythingInNamespace()
	{
		AssertEquals(
			"Class1Class2Interface1Interface2Structure1Structure2Delegate1Delegate2Enumeration1Enumeration2", 
			EvaluateNamespace(
				"<template>" +
					"<for-each-class-in-namespace><type-name /></for-each-class-in-namespace>" +
					"<for-each-interface-in-namespace><type-name /></for-each-interface-in-namespace>" +
					"<for-each-structure-in-namespace><type-name /></for-each-structure-in-namespace>" +
					"<for-each-delegate-in-namespace><type-name /></for-each-delegate-in-namespace>" +
					"<for-each-enumeration-in-namespace><type-name /></for-each-enumeration-in-namespace>" +
				"</template>",
				"NDoc.Test.TwoOfEverything"));
	}

	public void TestTypeLinkVariable()
	{
		AssertEquals(
			"<a href=\"NDoc.Test.Class1.html\">Class1</a>",
			EvaluateType(
				"<a href=\"{$type-link}\"><type-name /></a>",
				"NDoc.Test",
				"Class1"));
	}

	public void TestTypeSummaryWithoutPara()
	{
		AssertEquals(
			"This summary has no para element.",
			EvaluateType(
				"<type-summary />",
				"NDoc.Test.Summaries",
				"SummaryWithoutPara"));
	}

	public void TestTypeSummaryWithPara()
	{
		AssertEquals(
			"<p>This summary has one para element.</p>",
			EvaluateType(
				"<type-summary />",
				"NDoc.Test.Summaries",
				"SummaryWithPara"));
	}

	public void TestNoTypeSummary()
	{
		AssertEquals(
			String.Empty,
			EvaluateType(
				"<type-summary />",
				"NDoc.Test.Summaries",
				"NoSummary"));
	}
}
