# Generated by SLiNgshoT at 3/12/2002 10:05:57 PM

!IFNDEF CFG

CFG = Debug

!ENDIF

!IF "$(CFG)" == "Debug"

GUI_DIRECTORY = Gui\bin\Debug
GUI = $(GUI_DIRECTORY)\NDocGui.exe
GUI_DEBUG = /debug+
GUI_DOC = /doc:$(GUI_DIRECTORY)\NDocGui.xml

CONSOLE_DIRECTORY = Console\bin\Debug
CONSOLE = $(CONSOLE_DIRECTORY)\NDocConsole.exe
CONSOLE_DEBUG = /debug+
CONSOLE_DOC = /doc:$(CONSOLE_DIRECTORY)\NDocConsole.xml

CORE_DIRECTORY = Core\bin\Debug
CORE = $(CORE_DIRECTORY)\NDoc.Core.dll
CORE_DEBUG = /debug+
CORE_DOC = /doc:$(CORE_DIRECTORY)\NDoc.Core.xml

XML_DIRECTORY = Documenter\Xml\bin\Debug
XML = $(XML_DIRECTORY)\NDoc.Documenter.Xml.dll
XML_DEBUG = /debug+
XML_DOC = /doc:$(XML_DIRECTORY)\NDoc.Documenter.Xml.xml

VISUALSTUDIO_DIRECTORY = VisualStudio\bin\Debug
VISUALSTUDIO = $(VISUALSTUDIO_DIRECTORY)\NDoc.VisualStudio.dll
VISUALSTUDIO_DEBUG = /debug+
VISUALSTUDIO_DOC = /doc:$(VISUALSTUDIO_DIRECTORY)\NDoc.VisualStudio.xml

TEST_DIRECTORY = Test\bin\Debug
TEST = $(TEST_DIRECTORY)\NDoc.Test.dll
TEST_DEBUG = /debug+
TEST_DOC = /doc:$(TEST_DIRECTORY)\NDoc.Test.xml

MSDN_DIRECTORY = Documenter\Msdn\bin\Debug
MSDN = $(MSDN_DIRECTORY)\NDoc.Documenter.Msdn.dll
MSDN_DEBUG = /debug+
MSDN_DOC = /doc:$(MSDN_DIRECTORY)\NDoc.Documenter.Msdn.xml

NDOCBUILD_DIRECTORY = Addins\NDocBuild\bin\Debug
NDOCBUILD = $(NDOCBUILD_DIRECTORY)\NDocBuild.dll
NDOCBUILD_DEBUG = /debug+
NDOCBUILD_DOC = /doc:$(NDOCBUILD_DIRECTORY)\NDocBuild.xml

JAVADOC_DIRECTORY = Documenter\JavaDoc\bin\Debug
JAVADOC = $(JAVADOC_DIRECTORY)\NDoc.Documenter.JavaDoc.dll
JAVADOC_DEBUG = /debug+
JAVADOC_DOC = /doc:$(JAVADOC_DIRECTORY)\NDoc.Documenter.JavaDoc.xml

!ELSE IF "$(CFG)" == "Release"

GUI_DIRECTORY = Gui\bin\Release
GUI = $(GUI_DIRECTORY)\NDocGui.exe
GUI_DEBUG = /debug-
GUI_DOC = /doc:$(GUI_DIRECTORY)\NDocGui.xml

CONSOLE_DIRECTORY = Console\bin\Release
CONSOLE = $(CONSOLE_DIRECTORY)\NDocConsole.exe
CONSOLE_DEBUG = /debug-
CONSOLE_DOC = /doc:$(CONSOLE_DIRECTORY)\NDocConsole.xml

CORE_DIRECTORY = Core\bin\Release
CORE = $(CORE_DIRECTORY)\NDoc.Core.dll
CORE_DEBUG = /debug-
CORE_DOC = /doc:$(CORE_DIRECTORY)\NDoc.Core.xml

XML_DIRECTORY = Documenter\Xml\bin\Release
XML = $(XML_DIRECTORY)\NDoc.Documenter.Xml.dll
XML_DEBUG = /debug-
XML_DOC = /doc:$(XML_DIRECTORY)\NDoc.Documenter.Xml.xml

VISUALSTUDIO_DIRECTORY = VisualStudio\bin\Release
VISUALSTUDIO = $(VISUALSTUDIO_DIRECTORY)\NDoc.VisualStudio.dll
VISUALSTUDIO_DEBUG = /debug-
VISUALSTUDIO_DOC = /doc:$(VISUALSTUDIO_DIRECTORY)\NDoc.VisualStudio.xml

TEST_DIRECTORY = Test\bin\Release
TEST = $(TEST_DIRECTORY)\NDoc.Test.dll
TEST_DEBUG = /debug-
TEST_DOC = /doc:$(TEST_DIRECTORY)\NDoc.Test.xml

MSDN_DIRECTORY = Documenter\Msdn\bin\Release
MSDN = $(MSDN_DIRECTORY)\NDoc.Documenter.Msdn.dll
MSDN_DEBUG = /debug-
MSDN_DOC = /doc:$(MSDN_DIRECTORY)\NDoc.Documenter.Msdn.xml

NDOCBUILD_DIRECTORY = Addins\NDocBuild\bin\Release
NDOCBUILD = $(NDOCBUILD_DIRECTORY)\NDocBuild.dll
NDOCBUILD_DEBUG = /debug-
NDOCBUILD_DOC =

JAVADOC_DIRECTORY = Documenter\JavaDoc\bin\Release
JAVADOC = $(JAVADOC_DIRECTORY)\NDoc.Documenter.JavaDoc.dll
JAVADOC_DEBUG = /debug-
JAVADOC_DOC = /doc:$(JAVADOC_DIRECTORY)\NDoc.Documenter.JavaDoc.xml

!ENDIF

all: $(GUI) $(CONSOLE) $(CORE) $(XML) $(VISUALSTUDIO) $(TEST) $(MSDN) $(NDOCBUILD) $(JAVADOC)

GUI_SOURCE_FILES = \
	Gui\AboutForm.cs \
	Gui\AssemblyInfo.cs \
	Gui\AssemblySlashDocForm.cs \
	Gui\BuildWorker.cs \
	Gui\ErrorForm.cs \
	Gui\HeaderGroupBox.cs \
	Gui\MainForm.cs \
	Gui\NamespaceSummariesForm.cs \
	Gui\SolutionForm.cs

GUI_RESX_FILES = \
	Gui\AboutForm.resx \
	Gui\AssemblySlashDocForm.resx \
	Gui\ErrorForm.resx \
	Gui\MainForm.resx \
	Gui\NamespaceSummariesForm.resx \
	Gui\SolutionForm.resx

GUI_RESOURCE_FILES = \
	Gui\About.rtf

CONSOLE_SOURCE_FILES = \
	Console\AssemblyInfo.cs \
	Console\Console.cs

CORE_SOURCE_FILES = \
	Core\AssemblyInfo.cs \
	Core\AssemblyResolver.cs \
	Core\AssemblySlashDoc.cs \
	Core\BaseDocumenter.cs \
	Core\BaseDocumenterConfig.cs \
	Core\DocumenterException.cs \
	Core\EmbeddedResources.cs \
	Core\HtmlHelp.cs \
	Core\IDocumenter.cs \
	Core\IDocumenterConfig.cs \
	Core\Project.cs

XML_SOURCE_FILES = \
	Documenter\Xml\AssemblyInfo.cs \
	Documenter\Xml\XmlDocumenter.cs \
	Documenter\Xml\XmlDocumenterConfig.cs

VISUALSTUDIO_SOURCE_FILES = \
	VisualStudio\AssemblyInfo.cs \
	VisualStudio\Project.cs \
	VisualStudio\ProjectConfig.cs \
	VisualStudio\Solution.cs

TEST_SOURCE_FILES = \
	Test\AssemblyInfo.cs \
	Test\Test.cs

MSDN_SOURCE_FILES = \
	Documenter\Msdn\AssemblyInfo.cs \
	Documenter\Msdn\MsdnDocumenter.cs \
	Documenter\Msdn\MsdnDocumenterConfig.cs

MSDN_RESOURCE_FILES = \
	Documenter\Msdn\css\MSDN.css \
	Documenter\Msdn\xslt\allmembers.xslt \
	Documenter\Msdn\xslt\common.xslt \
	Documenter\Msdn\xslt\event.xslt \
	Documenter\Msdn\xslt\field.xslt \
	Documenter\Msdn\xslt\filenames.xslt \
	Documenter\Msdn\xslt\individualmembers.xslt \
	Documenter\Msdn\xslt\member.xslt \
	Documenter\Msdn\xslt\memberoverload.xslt \
	Documenter\Msdn\xslt\memberscommon.xslt \
	Documenter\Msdn\xslt\namespace.xslt \
	Documenter\Msdn\xslt\namespacehierarchy.xslt \
	Documenter\Msdn\xslt\property.xslt \
	Documenter\Msdn\xslt\syntax.xslt \
	Documenter\Msdn\xslt\tags.xslt \
	Documenter\Msdn\xslt\type.xslt \
	Documenter\Msdn\xslt\vb-syntax.xslt

NDOCBUILD_SOURCE_FILES = \
	Addins\NDocBuild\AssemblyInfo.cs \
	Addins\NDocBuild\BuildWorker.cs \
	Addins\NDocBuild\Connect.cs \
	Addins\NDocBuild\NamespaceSummariesForm.cs \
	Addins\NDocBuild\SolutionSettingsForm.cs

NDOCBUILD_RESX_FILES = \
	Addins\NDocBuild\NamespaceSummariesForm.resx \
	Addins\NDocBuild\SolutionSettingsForm.resx

JAVADOC_SOURCE_FILES = \
	Documenter\JavaDoc\AssemblyInfo.cs \
	Documenter\JavaDoc\JavaDocDocumenter.cs \
	Documenter\JavaDoc\JavaDocDocumenterConfig.cs

JAVADOC_RESOURCE_FILES = \
	Documenter\JavaDoc\css\JavaDoc.css \
	Documenter\JavaDoc\xslt\javadoc.xslt \
	Documenter\JavaDoc\xslt\namespace-summary.xslt \
	Documenter\JavaDoc\xslt\overview-summary.xslt \
	Documenter\JavaDoc\xslt\type.xslt

$(GUI): $(CORE) $(MSDN) $(XML) $(JAVADOC) $(VISUALSTUDIO) $(GUI_SOURCE_FILES) $(GUI_RESX_FILES) $(GUI_RESOURCE_FILES)
	if not exist $(GUI_DIRECTORY) md $(GUI_DIRECTORY)
	resgen Gui\AboutForm.resx $(GUI_DIRECTORY)\NDoc.Gui.AboutForm.resources
	resgen Gui\AssemblySlashDocForm.resx $(GUI_DIRECTORY)\NDoc.Gui.AssemblySlashDocForm.resources
	resgen Gui\ErrorForm.resx $(GUI_DIRECTORY)\NDoc.Gui.ErrorForm.resources
	resgen Gui\MainForm.resx $(GUI_DIRECTORY)\NDoc.Gui.MainForm.resources
	resgen Gui\NamespaceSummariesForm.resx $(GUI_DIRECTORY)\NDoc.Gui.NamespaceSummariesForm.resources
	resgen Gui\SolutionForm.resx $(GUI_DIRECTORY)\NDoc.Gui.SolutionForm.resources
	csc /nologo /target:winexe /out:$(GUI) $(GUI_DEBUG) $(GUI_DOC) $(GUI_SOURCE_FILES) /reference:$(CORE) /reference:$(MSDN) /reference:$(XML) /reference:$(JAVADOC) /reference:$(VISUALSTUDIO) /resource:$(GUI_DIRECTORY)\NDoc.Gui.AboutForm.resources /resource:$(GUI_DIRECTORY)\NDoc.Gui.AssemblySlashDocForm.resources /resource:$(GUI_DIRECTORY)\NDoc.Gui.ErrorForm.resources /resource:$(GUI_DIRECTORY)\NDoc.Gui.MainForm.resources /resource:$(GUI_DIRECTORY)\NDoc.Gui.NamespaceSummariesForm.resources /resource:$(GUI_DIRECTORY)\NDoc.Gui.SolutionForm.resources /resource:Gui\About.rtf,NDoc.Gui.About.rtf
	copy $(CORE) $(GUI_DIRECTORY)
	copy $(MSDN) $(GUI_DIRECTORY)
	copy $(XML) $(GUI_DIRECTORY)
	copy $(JAVADOC) $(GUI_DIRECTORY)
	copy $(VISUALSTUDIO) $(GUI_DIRECTORY)

$(CONSOLE): $(CORE) $(JAVADOC) $(MSDN) $(XML) $(CONSOLE_SOURCE_FILES)
	if not exist $(CONSOLE_DIRECTORY) md $(CONSOLE_DIRECTORY)
	csc /nologo /target:exe /out:$(CONSOLE) $(CONSOLE_DEBUG) $(CONSOLE_DOC) $(CONSOLE_SOURCE_FILES) /reference:$(CORE) /reference:$(JAVADOC) /reference:$(MSDN) /reference:$(XML)
	copy $(CORE) $(CONSOLE_DIRECTORY)
	copy $(JAVADOC) $(CONSOLE_DIRECTORY)
	copy $(MSDN) $(CONSOLE_DIRECTORY)
	copy $(XML) $(CONSOLE_DIRECTORY)

$(CORE): $(CORE_SOURCE_FILES)
	if not exist $(CORE_DIRECTORY) md $(CORE_DIRECTORY)
	csc /nologo /target:library /out:$(CORE) $(CORE_DEBUG) $(CORE_DOC) $(CORE_SOURCE_FILES)

$(XML): $(CORE) $(XML_SOURCE_FILES)
	if not exist $(XML_DIRECTORY) md $(XML_DIRECTORY)
	csc /nologo /target:library /out:$(XML) $(XML_DEBUG) $(XML_DOC) $(XML_SOURCE_FILES) /reference:$(CORE)
	copy $(CORE) $(XML_DIRECTORY)

$(VISUALSTUDIO): $(VISUALSTUDIO_SOURCE_FILES)
	if not exist $(VISUALSTUDIO_DIRECTORY) md $(VISUALSTUDIO_DIRECTORY)
	csc /nologo /target:library /out:$(VISUALSTUDIO) $(VISUALSTUDIO_DEBUG) $(VISUALSTUDIO_DOC) $(VISUALSTUDIO_SOURCE_FILES)

$(TEST): $(TEST_SOURCE_FILES)
	if not exist $(TEST_DIRECTORY) md $(TEST_DIRECTORY)
	csc /nologo /target:library /out:$(TEST) $(TEST_DEBUG) $(TEST_DOC) $(TEST_SOURCE_FILES)

$(MSDN): $(CORE) $(MSDN_SOURCE_FILES) $(MSDN_RESOURCE_FILES)
	if not exist $(MSDN_DIRECTORY) md $(MSDN_DIRECTORY)
	csc /nologo /target:library /out:$(MSDN) $(MSDN_DEBUG) $(MSDN_DOC) $(MSDN_SOURCE_FILES) /reference:$(CORE) /resource:Documenter\Msdn\css\MSDN.css,NDoc.Documenter.Msdn.css.MSDN.css /resource:Documenter\Msdn\xslt\allmembers.xslt,NDoc.Documenter.Msdn.xslt.allmembers.xslt /resource:Documenter\Msdn\xslt\common.xslt,NDoc.Documenter.Msdn.xslt.common.xslt /resource:Documenter\Msdn\xslt\event.xslt,NDoc.Documenter.Msdn.xslt.event.xslt /resource:Documenter\Msdn\xslt\field.xslt,NDoc.Documenter.Msdn.xslt.field.xslt /resource:Documenter\Msdn\xslt\filenames.xslt,NDoc.Documenter.Msdn.xslt.filenames.xslt /resource:Documenter\Msdn\xslt\individualmembers.xslt,NDoc.Documenter.Msdn.xslt.individualmembers.xslt /resource:Documenter\Msdn\xslt\member.xslt,NDoc.Documenter.Msdn.xslt.member.xslt /resource:Documenter\Msdn\xslt\memberoverload.xslt,NDoc.Documenter.Msdn.xslt.memberoverload.xslt /resource:Documenter\Msdn\xslt\memberscommon.xslt,NDoc.Documenter.Msdn.xslt.memberscommon.xslt /resource:Documenter\Msdn\xslt\namespace.xslt,NDoc.Documenter.Msdn.xslt.namespace.xslt /resource:Documenter\Msdn\xslt\namespacehierarchy.xslt,NDoc.Documenter.Msdn.xslt.namespacehierarchy.xslt /resource:Documenter\Msdn\xslt\property.xslt,NDoc.Documenter.Msdn.xslt.property.xslt /resource:Documenter\Msdn\xslt\syntax.xslt,NDoc.Documenter.Msdn.xslt.syntax.xslt /resource:Documenter\Msdn\xslt\tags.xslt,NDoc.Documenter.Msdn.xslt.tags.xslt /resource:Documenter\Msdn\xslt\type.xslt,NDoc.Documenter.Msdn.xslt.type.xslt /resource:Documenter\Msdn\xslt\vb-syntax.xslt,NDoc.Documenter.Msdn.xslt.vb-syntax.xslt
	copy $(CORE) $(MSDN_DIRECTORY)

$(NDOCBUILD): $(CORE) $(MSDN) $(XML) $(NDOCBUILD_SOURCE_FILES) $(NDOCBUILD_RESX_FILES)
	if not exist $(NDOCBUILD_DIRECTORY) md $(NDOCBUILD_DIRECTORY)
	resgen Addins\NDocBuild\NamespaceSummariesForm.resx $(NDOCBUILD_DIRECTORY)\NDocBuild.NamespaceSummariesForm.resources
	resgen Addins\NDocBuild\SolutionSettingsForm.resx $(NDOCBUILD_DIRECTORY)\NDocBuild.SolutionSettingsForm.resources
	csc /nologo /target:library /out:$(NDOCBUILD) $(NDOCBUILD_DEBUG) $(NDOCBUILD_DOC) $(NDOCBUILD_SOURCE_FILES) /reference:$(CORE) /reference:$(MSDN) /reference:$(XML) /resource:$(NDOCBUILD_DIRECTORY)\NDocBuild.NamespaceSummariesForm.resources /resource:$(NDOCBUILD_DIRECTORY)\NDocBuild.SolutionSettingsForm.resources
	copy $(CORE) $(NDOCBUILD_DIRECTORY)
	copy $(MSDN) $(NDOCBUILD_DIRECTORY)
	copy $(XML) $(NDOCBUILD_DIRECTORY)

$(JAVADOC): $(CORE) $(JAVADOC_SOURCE_FILES) $(JAVADOC_RESOURCE_FILES)
	if not exist $(JAVADOC_DIRECTORY) md $(JAVADOC_DIRECTORY)
	csc /nologo /target:library /out:$(JAVADOC) $(JAVADOC_DEBUG) $(JAVADOC_DOC) $(JAVADOC_SOURCE_FILES) /reference:$(CORE) /resource:Documenter\JavaDoc\css\JavaDoc.css,NDoc.Documenter.JavaDoc.css.JavaDoc.css /resource:Documenter\JavaDoc\xslt\javadoc.xslt,NDoc.Documenter.JavaDoc.xslt.javadoc.xslt /resource:Documenter\JavaDoc\xslt\namespace-summary.xslt,NDoc.Documenter.JavaDoc.xslt.namespace-summary.xslt /resource:Documenter\JavaDoc\xslt\overview-summary.xslt,NDoc.Documenter.JavaDoc.xslt.overview-summary.xslt /resource:Documenter\JavaDoc\xslt\type.xslt,NDoc.Documenter.JavaDoc.xslt.type.xslt
	copy $(CORE) $(JAVADOC_DIRECTORY)

clean:
	rd /s /q $(GUI_DIRECTORY)
	rd /s /q $(CONSOLE_DIRECTORY)
	rd /s /q $(CORE_DIRECTORY)
	rd /s /q $(XML_DIRECTORY)
	rd /s /q $(VISUALSTUDIO_DIRECTORY)
	rd /s /q $(TEST_DIRECTORY)
	rd /s /q $(MSDN_DIRECTORY)
	rd /s /q $(NDOCBUILD_DIRECTORY)
	rd /s /q $(JAVADOC_DIRECTORY)
