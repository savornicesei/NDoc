﻿<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<!-- -->
	<xsl:output method="html" indent="yes" />
	<!-- -->
	<xsl:include href="common.xslt" />
	<xsl:include href="memberscommon.xslt" />
	<!-- -->
	<xsl:param name='id' />
	<!-- -->
	<xsl:template name="type-members">
		<xsl:param name="type" />
		<html dir="LTR">
			<xsl:call-template name="html-head">
				<xsl:with-param name="title" select="concat(@name, ' Members')" />
			</xsl:call-template>
			<body>
				<xsl:call-template name="title-row">
					<xsl:with-param name="type-name">
						<xsl:value-of select="@name" /> Members
					</xsl:with-param>
				</xsl:call-template>
				<div id="content">
					<!-- public static members -->
					<xsl:call-template name="public-static-section">
						<xsl:with-param name="member" select="'field'" />
					</xsl:call-template>
					<xsl:call-template name="public-static-section">
						<xsl:with-param name="member" select="'property'" />
					</xsl:call-template>
					<xsl:call-template name="public-static-section">
						<xsl:with-param name="member" select="'method'" />
					</xsl:call-template>
					<xsl:call-template name="public-static-section">
						<xsl:with-param name="member" select="'operator'" />
					</xsl:call-template>
					<xsl:call-template name="public-static-section">
						<xsl:with-param name="member" select="'event'" />
					</xsl:call-template>
					<!-- protected static members -->
					<xsl:call-template name="protected-static-section">
						<xsl:with-param name="member" select="'field'" />
					</xsl:call-template>
					<xsl:call-template name="protected-static-section">
						<xsl:with-param name="member" select="'property'" />
					</xsl:call-template>
					<xsl:call-template name="protected-static-section">
						<xsl:with-param name="member" select="'method'" />
					</xsl:call-template>
					<xsl:call-template name="protected-static-section">
						<xsl:with-param name="member" select="'event'" />
					</xsl:call-template>
					<!-- protected internal static members -->
					<xsl:call-template name="protected-internal-static-section">
						<xsl:with-param name="member" select="'field'" />
					</xsl:call-template>
					<xsl:call-template name="protected-internal-static-section">
						<xsl:with-param name="member" select="'property'" />
					</xsl:call-template>
					<xsl:call-template name="protected-internal-static-section">
						<xsl:with-param name="member" select="'method'" />
					</xsl:call-template>
					<xsl:call-template name="protected-internal-static-section">
						<xsl:with-param name="member" select="'event'" />
					</xsl:call-template>
					<!-- internal static members -->
					<xsl:call-template name="internal-static-section">
						<xsl:with-param name="member" select="'field'" />
					</xsl:call-template>
					<xsl:call-template name="internal-static-section">
						<xsl:with-param name="member" select="'property'" />
					</xsl:call-template>
					<xsl:call-template name="internal-static-section">
						<xsl:with-param name="member" select="'method'" />
					</xsl:call-template>
					<xsl:call-template name="internal-static-section">
						<xsl:with-param name="member" select="'event'" />
					</xsl:call-template>
					<!-- private static members -->
					<xsl:call-template name="private-static-section">
						<xsl:with-param name="member" select="'field'" />
					</xsl:call-template>
					<xsl:call-template name="private-static-section">
						<xsl:with-param name="member" select="'property'" />
					</xsl:call-template>
					<xsl:call-template name="private-static-section">
						<xsl:with-param name="member" select="'method'" />
					</xsl:call-template>
					<xsl:call-template name="private-static-section">
						<xsl:with-param name="member" select="'event'" />
					</xsl:call-template>
					<!-- public instance members -->
					<xsl:if test="constructor[@access='Public']">
						<h4>Public Instance Constructors</h4>
						<div class="table">
							<table cellspacing="0">
								<xsl:apply-templates select="constructor[@access='Public']" />
							</table>
						</div>
					</xsl:if>
					<xsl:call-template name="public-instance-section">
						<xsl:with-param name="member" select="'field'" />
					</xsl:call-template>
					<xsl:call-template name="public-instance-section">
						<xsl:with-param name="member" select="'property'" />
					</xsl:call-template>
					<xsl:call-template name="public-instance-section">
						<xsl:with-param name="member" select="'method'" />
					</xsl:call-template>
					<xsl:call-template name="public-instance-section">
						<xsl:with-param name="member" select="'event'" />
					</xsl:call-template>
					<!-- protected instance members -->
					<xsl:if test="constructor[@access='Family']">
						<h4>Protected Instance Constructors</h4>
						<div class="table">
							<table cellspacing="0">
								<xsl:apply-templates select="constructor[@access='Family']" />
							</table>
						</div>
					</xsl:if>
					<xsl:call-template name="protected-instance-section">
						<xsl:with-param name="member" select="'field'" />
					</xsl:call-template>
					<xsl:call-template name="protected-instance-section">
						<xsl:with-param name="member" select="'property'" />
					</xsl:call-template>
					<xsl:call-template name="protected-instance-section">
						<xsl:with-param name="member" select="'method'" />
					</xsl:call-template>
					<xsl:call-template name="protected-instance-section">
						<xsl:with-param name="member" select="'event'" />
					</xsl:call-template>
					<!-- protected internal instance members -->
					<xsl:if test="constructor[@access='FamilyOrAssembly']">
						<h4>Protected Internal Instance Constructors</h4>
						<div class="table">
							<table cellspacing="0">
								<xsl:apply-templates select="constructor[@access='FamilyOrAssembly']" />
							</table>
						</div>
					</xsl:if>
					<xsl:call-template name="protected-internal-instance-section">
						<xsl:with-param name="member" select="'field'" />
					</xsl:call-template>
					<xsl:call-template name="protected-internal-instance-section">
						<xsl:with-param name="member" select="'property'" />
					</xsl:call-template>
					<xsl:call-template name="protected-internal-instance-section">
						<xsl:with-param name="member" select="'method'" />
					</xsl:call-template>
					<xsl:call-template name="protected-internal-instance-section">
						<xsl:with-param name="member" select="'event'" />
					</xsl:call-template>
					<!-- internal instance members -->
					<xsl:if test="constructor[@access='Assembly']">
						<h4>Internal Instance Constructors</h4>
						<div class="table">
							<table cellspacing="0">
								<xsl:apply-templates select="constructor[@access='Assembly']" />
							</table>
						</div>
					</xsl:if>
					<xsl:call-template name="internal-instance-section">
						<xsl:with-param name="member" select="'field'" />
					</xsl:call-template>
					<xsl:call-template name="internal-instance-section">
						<xsl:with-param name="member" select="'property'" />
					</xsl:call-template>
					<xsl:call-template name="internal-instance-section">
						<xsl:with-param name="member" select="'method'" />
					</xsl:call-template>
					<xsl:call-template name="internal-instance-section">
						<xsl:with-param name="member" select="'event'" />
					</xsl:call-template>
					<!-- private instance members -->
					<xsl:if test="constructor[@access='Private']">
						<h4>Private Instance Constructors</h4>
						<div class="table">
							<table cellspacing="0">
								<xsl:apply-templates select="constructor[@access='Private']" />
							</table>
						</div>
					</xsl:if>
					<xsl:call-template name="private-instance-section">
						<xsl:with-param name="member" select="'field'" />
					</xsl:call-template>
					<xsl:call-template name="private-instance-section">
						<xsl:with-param name="member" select="'property'" />
					</xsl:call-template>
					<xsl:call-template name="private-instance-section">
						<xsl:with-param name="member" select="'method'" />
					</xsl:call-template>
					<xsl:call-template name="private-instance-section">
						<xsl:with-param name="member" select="'event'" />
					</xsl:call-template>
					<xsl:call-template name="seealso-section">
						<xsl:with-param name="page">members</xsl:with-param>
					</xsl:call-template>
				</div>
			</body>
		</html>
	</xsl:template>
	<!-- -->
	<xsl:template match="constructor">
		<xsl:variable name="access" select="@access" />
		<xsl:if test="not(preceding-sibling::constructor[@access=$access])">
			<tr VALIGN="top">
				<xsl:choose>
					<xsl:when test="count(../constructor) &gt; 1">
						<td width="50%">
							<a>
								<xsl:attribute name="href">
									<xsl:call-template name="get-filename-for-current-constructor-overloads" />
								</xsl:attribute>
								<xsl:value-of select="../@name" />
							</a>
						</td>
						<td width="50%">Overloaded. Initialize a new instance of the <xsl:value-of select="../@name" /> class.</td>
					</xsl:when>
					<xsl:otherwise>
						<td width="50%">
							<a>
								<xsl:attribute name="href">
									<xsl:call-template name="get-filename-for-current-constructor" />
								</xsl:attribute>
								<xsl:value-of select="../@name" />
								<xsl:text> Constructor</xsl:text>
							</a>
						</td>
						<td width="50%">
							<xsl:apply-templates select="documentation/summary/node()" mode="slashdoc" />
						</td>
					</xsl:otherwise>
				</xsl:choose>
			</tr>
		</xsl:if>
	</xsl:template>
	<!-- -->
</xsl:stylesheet>