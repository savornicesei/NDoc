<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:MSHelp="http://msdn.microsoft.com/mshelp"
	xmlns:NUtil="urn:ndoc-sourceforge-net:documenters.NativeHtmlHelp2.xsltUtilities"
	exclude-result-prefixes="NUtil">	
	<!-- -->
	<xsl:output method="xml" indent="no" encoding="utf-8" omit-xml-declaration="yes"/>
	<!-- -->
	<xsl:include href="common.xslt" />
	<!-- -->
	<xsl:param name='namespace' />
	<!-- -->
	<xsl:template match="/">
		<xsl:variable name="ns" select="ndoc/assembly/module/namespace[@name=$namespace]" />
		<html dir="LTR">
			<xsl:call-template name="html-head">
				<xsl:with-param name="title" select="concat($ns/@name, 'Hierarchy')" />
				<xsl:with-param name="page-type" select="'hierarchy'"/>
			</xsl:call-template>
			<body topmargin="0" id="bodyID" class="dtBODY">
				<object id="obj_cook" classid="clsid:59CC0C20-679B-11D2-88BD-0800361A1803" style="display:none;"></object>
				<xsl:call-template name="title-row">
					<xsl:with-param name="type-name" select="concat($ns/@name, ' Hierarchy')" />
				</xsl:call-template>
				<div id="nstext" valign="bottom">
					<p>
						<xsl:call-template name="get-xlink-for-foreign-type">
							<xsl:with-param name="type" select="'T:System.Object'" />									
						</xsl:call-template>
					</p>
					<xsl:variable name="roots" select="$ns//*[(local-name()='class' and not(base)) or (local-name()='base' and not(base))]" />
					<xsl:call-template name="call-draw">
						<xsl:with-param name="nodes" select="$roots" />
						<xsl:with-param name="level" select="1" />
					</xsl:call-template>
					<xsl:if test="$ns/interface">
						<h4 class="dtH4">Interfaces</h4>
						<xsl:apply-templates select="$ns/interface">
							<xsl:sort select="@name" />
						</xsl:apply-templates>
					</xsl:if>
					
					
					<h4 class="dtH4">See Also</h4>
					<p>
						<a href="{NUtil:GetNamespaceHRef( string( $namespace ) )}">
							<xsl:value-of select="$namespace" /> Namespace
						</a>
					</p>
					
					
					<xsl:call-template name="footer-row">
						<xsl:with-param name="type-name" select="concat($ns/@name, ' Hierarchy')" />
					</xsl:call-template>
				</div>
			</body>
		</html>
	</xsl:template>
	<!-- -->
	<xsl:template name="call-draw">
		<xsl:param name="nodes" />
		<xsl:param name="level" />
		<xsl:for-each select="$nodes">
			<xsl:sort select="@name" />
			<xsl:if test="position() = 1">
				<xsl:variable name="head" select="." />
				<xsl:call-template name="draw">
					<xsl:with-param name="head" select="$head" />
					<xsl:with-param name="tail" select="$nodes[@name != $head/@name]" />
					<xsl:with-param name="level" select="$level" />
				</xsl:call-template>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	<!-- -->
	<xsl:template name="draw">
		<xsl:param name="head" />
		<xsl:param name="tail" />
		<xsl:param name="level" />
		<p>
			<xsl:call-template name="indent">
				<xsl:with-param name="count" select="$level" />
			</xsl:call-template>
			<xsl:choose>
				<xsl:when test="starts-with($head/@id, 'T:System.')">
					<xsl:call-template name="get-xlink-for-foreign-type">
						<xsl:with-param name="type" select="$head/@id" />									
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:variable name="base-class-id" select="$head/@id" />
					<xsl:variable name="base-class" select="//class[@id=$base-class-id]" />
					<xsl:choose>
						<xsl:when test="$base-class">
						<a href="{NUtil:GetLocalCRef( string( $base-class-id ) ) }">
								<xsl:call-template name="strip-namespace">
									<xsl:with-param name="name" select="$head/@type" />
								</xsl:call-template>
								<xsl:value-of select="substring-after($head/@id, 'T:')"/>
							</a>
						</xsl:when>
						<xsl:otherwise>
							<xsl:call-template name="strip-namespace">
								<xsl:with-param name="name" select="$head/@type" />
							</xsl:call-template>
							<xsl:value-of select="substring-after($head/@id, 'T:')"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</p>
		<xsl:variable name="derivatives" select="/ndoc/assembly/module/namespace/class[base/@id = $head/@id] | /ndoc/assembly/module/namespace/class/descendant::base[base[@id = $head/@id]]" />
		<xsl:if test="$derivatives">
			<xsl:call-template name="call-draw">
				<xsl:with-param name="nodes" select="$derivatives" />
				<xsl:with-param name="level" select="$level + 1" />
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="$tail">
			<xsl:call-template name="call-draw">
				<xsl:with-param name="nodes" select="$tail" />
				<xsl:with-param name="level" select="$level" />
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="indent">
		<xsl:param name="count" />
		<xsl:if test="$count &gt; 0">
			<xsl:text>&#160;&#160;&#160;&#160;</xsl:text>
			<xsl:call-template name="indent">
				<xsl:with-param name="count" select="$count - 1" />
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template match="interface">
		<p>
			<a href="{NUtil:GetLocalCRef( string( @id ) ) }">
				<xsl:value-of select="@name" />
			</a>
		</p>
	</xsl:template>
	<!-- -->
</xsl:stylesheet>
