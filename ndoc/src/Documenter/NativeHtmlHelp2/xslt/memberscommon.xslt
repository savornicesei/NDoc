<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:MSHelp="http://msdn.microsoft.com/mshelp"
	xmlns:NUtil="urn:ndoc-sourceforge-net:documenters.NativeHtmlHelp2.xsltUtilities"
	exclude-result-prefixes="NUtil" >
	<!-- -->
	<xsl:template match="/">
		<xsl:apply-templates select="ndoc/assembly/module/namespace/*[@id=$id]" />
	</xsl:template>
	<!-- -->
	<xsl:template match="class">
		<xsl:call-template name="type-members">
			<xsl:with-param name="type">Class</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<!-- -->
	<xsl:template match="interface">
		<xsl:call-template name="type-members">
			<xsl:with-param name="type">Interface</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<!-- -->
	<xsl:template match="structure">
		<xsl:call-template name="type-members">
			<xsl:with-param name="type">Structure</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<!-- -->
	<xsl:template name="get-big-member-plural">
		<xsl:param name="member" />
		<xsl:choose>
			<xsl:when test="$member='field'">Fields</xsl:when>
			<xsl:when test="$member='property'">Properties</xsl:when>
			<xsl:when test="$member='event'">Events</xsl:when>
			<xsl:when test="$member='operator'">Operators</xsl:when>
			<xsl:otherwise>Methods</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- -->
	<xsl:template name="get-small-member-plural">
		<xsl:param name="member" />
		<xsl:choose>
			<xsl:when test="$member='field'">fields</xsl:when>
			<xsl:when test="$member='property'">properties</xsl:when>
			<xsl:when test="$member='event'">events</xsl:when>
			<xsl:when test="$member='operator'">operators</xsl:when>
			<xsl:otherwise>methods</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- -->
	<xsl:template name="public-static-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Public' and @contract='Static']">
			<h4 class="dtH4">
				<xsl:text>Public Static </xsl:text>
  				<xsl:text>(Shared) </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Public' and @contract='Static']">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="protected-static-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Family' and @contract='Static']">
			<h4 class="dtH4">
				<xsl:text>Protected Static </xsl:text>
  				<xsl:text>(Shared) </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Family' and @contract='Static']">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="protected-internal-static-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='FamilyOrAssembly' and @contract='Static']">
			<h4 class="dtH4">
				<xsl:text>Protected Internal Static </xsl:text>
  				<xsl:text>(Shared) </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='FamilyOrAssembly' and @contract='Static']">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="internal-static-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Assembly' and @contract='Static']">
			<h4 class="dtH4">
				<xsl:text>Internal Static </xsl:text>
  				<xsl:text>(Shared) </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Assembly' and @contract='Static']">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="private-static-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Private' and @contract='Static']">
			<h4 class="dtH4">
				<xsl:text>Private Static </xsl:text>
  				<xsl:text>(Shared) </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Private' and @contract='Static']">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="public-instance-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Public' and not(@contract='Static')]">
			<h4 class="dtH4">
				<xsl:text>Public Instance </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Public' and not(@contract='Static')]">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="protected-instance-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Family' and not(@contract='Static')]">
			<h4 class="dtH4">
				<xsl:text>Protected Instance </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Family' and not(@contract='Static')]">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="protected-internal-instance-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='FamilyOrAssembly' and not(@contract='Static')]">
			<h4 class="dtH4">
				<xsl:text>Protected Internal Instance </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='FamilyOrAssembly' and not(@contract='Static')]">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="internal-instance-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Assembly' and not(@contract='Static')]">
			<h4 class="dtH4">
				<xsl:text>Internal Instance </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Assembly' and not(@contract='Static')]">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="private-instance-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Private' and not(@contract='Static') and not(@interface)]">
			<h4 class="dtH4">
				<xsl:text>Private Instance </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Private' and not(@contract='Static') and not(@interface)]">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="explicit-interface-implementations">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Private' and not(@contract='Static') and @interface]">
			<h4 class="dtH4">
				<xsl:text>Explicit Interface Implementations</xsl:text>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Private' and not(@contract='Static') and @interface]">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="images">
		<xsl:param name="access" />
		<xsl:param name="local-name" />
		<xsl:param name="contract" />
		<xsl:choose>
			<xsl:when test="$access='Public'">
				<img>
					<xsl:attribute name="src">
						<xsl:text>pub</xsl:text>
						<xsl:value-of select="$local-name"/>
						<xsl:text>.gif</xsl:text>
					</xsl:attribute>
				</img>
			</xsl:when>
			<xsl:when test="$access='Family'">
				<img>
					<xsl:attribute name="src">
						<xsl:text>prot</xsl:text>
						<xsl:value-of select="$local-name"/>
						<xsl:text>.gif</xsl:text>
					</xsl:attribute>
				</img>
			</xsl:when>
			<xsl:when test="$access='Private'">
				<img>
					<xsl:attribute name="src">
						<xsl:text>priv</xsl:text>
						<xsl:value-of select="$local-name"/>
						<xsl:text>.gif</xsl:text>
					</xsl:attribute>
				</img>
			</xsl:when>
			<xsl:when test="$access='Assembly' or $access='FamilyOrAssembly'">
				<img>
					<xsl:attribute name="src">
						<xsl:text>int</xsl:text>
						<xsl:value-of select="$local-name"/>
						<xsl:text>.gif</xsl:text>
					</xsl:attribute>
				</img>
			</xsl:when>
		</xsl:choose>
		<xsl:if test="$contract='Static'">
			<img src="static.gif" />
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template match="property[@declaringType]">
		<xsl:text>&#10;</xsl:text>
		<tr VALIGN="top">
			<td width="50%">
				<xsl:call-template name="images">
					<xsl:with-param name="access" select="@access" />
					<xsl:with-param name="contract" select="@contract" />
					<xsl:with-param name="local-name" select="local-name()" />
				</xsl:call-template>
				
				<xsl:call-template name="get-link-for-member">
					<xsl:with-param name="link-text" select="@name"/>
					<xsl:with-param name="member" select="."/>
				</xsl:call-template>
				
				<xsl:text> (inherited from </xsl:text>
				<b>
					<xsl:call-template name="strip-namespace">
						<xsl:with-param name="name" select="@declaringType" />
					</xsl:call-template>
				</b>
				<xsl:text>)</xsl:text>
			</td>
			<td width="50%">
				<xsl:call-template name="summary-with-no-paragraph" />
			</td>
		</tr>
	</xsl:template>
	<!-- -->
	<xsl:template match="field[@declaringType]">
		<xsl:text>&#10;</xsl:text>
		<tr VALIGN="top">
			<td width="50%">
				<xsl:call-template name="images">
					<xsl:with-param name="access" select="@access" />
					<xsl:with-param name="contract" select="@contract" />
					<xsl:with-param name="local-name" select="local-name()" />
				</xsl:call-template>
					
				<xsl:call-template name="get-link-for-member">
					<xsl:with-param name="link-text" select="@name"/>
					<xsl:with-param name="member" select="."/>
				</xsl:call-template>
				
				<xsl:text> (inherited from </xsl:text>
				<b>
					<xsl:call-template name="strip-namespace">
						<xsl:with-param name="name" select="@declaringType" />
					</xsl:call-template>
				</b>
				<xsl:text>)</xsl:text>
			</td>
			<td width="50%">
				<xsl:call-template name="summary-with-no-paragraph" />
			</td>
		</tr>
	</xsl:template>
	<!-- -->
	<xsl:template match="method[@declaringType]">
	
		<xsl:if test="not(NUtil:HasSimilarOverloads(concat(@name,':',@declaringType,':',@access,':',(@contract='Static'))))">
			<xsl:text>&#10;</xsl:text>
			<tr VALIGN="top">
				<td width="50%">
					<xsl:call-template name="images">
						<xsl:with-param name="access" select="@access" />
						<xsl:with-param name="contract" select="@contract" />
						<xsl:with-param name="local-name" select="local-name()" />
					</xsl:call-template>
					
					<xsl:call-template name="get-link-for-member">
						<xsl:with-param name="link-text" select="@name"/>
						<xsl:with-param name="member" select="."/>
					</xsl:call-template>

					<xsl:text> (inherited from </xsl:text>
					<b>
						<xsl:call-template name="strip-namespace">
							<xsl:with-param name="name" select="@declaringType" />
						</xsl:call-template>
					</b>
					<xsl:text>)</xsl:text>
				</td>
				<td width="50%">
					<xsl:if test="@contract='Override'">
						<xsl:text>Overidden. </xsl:text>
					</xsl:if>
					<xsl:if test="@overload">
						<xsl:text>Overloaded. </xsl:text>
					</xsl:if>
					<xsl:call-template name="summary-with-no-paragraph" />
				</td>
			</tr>
		</xsl:if>
	</xsl:template>

	<xsl:template match="event[@declaringType]">
		<xsl:text>&#10;</xsl:text>
		<tr VALIGN="top">
			<td width="50%">
				<xsl:call-template name="images">
					<xsl:with-param name="access" select="@access" />
					<xsl:with-param name="contract" select="@contract" />
					<xsl:with-param name="local-name" select="local-name()" />
				</xsl:call-template>
				
				<xsl:call-template name="get-link-for-member">
					<xsl:with-param name="link-text" select="@name"/>
					<xsl:with-param name="member" select="."/>
				</xsl:call-template>	
				
				<xsl:text> (inherited from </xsl:text>
				<b>
					<xsl:call-template name="get-datatype">
						<xsl:with-param name="datatype" select="@declaringType" />
						<xsl:with-param name="lang" select="'C#'"/>								
					</xsl:call-template>
				</b>
				<xsl:text>)</xsl:text>
			</td>
			<td width="50%">
				<xsl:call-template name="summary-with-no-paragraph" />
			</td>
		</tr>
	</xsl:template>
	<!-- -->
	<xsl:template match="field[not(@declaringType)]|property[not(@declaringType)]|event[not(@declaringType)]|method[not(@declaringType)]|operator">
		<xsl:variable name="member" select="local-name()" />
		<xsl:variable name="name" select="@name" />
		<xsl:variable name="contract" select="@contract" />
		<xsl:variable name="access" select="@access" />
		<xsl:if test="@name='op_Implicit' or @name='op_Explicit' or not(NUtil:HasSimilarOverloads(concat($name,'::',$access,':',($contract='Static'))))">
			<xsl:text>&#10;</xsl:text>
			<tr VALIGN="top">
				<xsl:choose>
					<xsl:when test="@name!='op_Implicit' and @name!='op_Explicit' and following-sibling::*[(local-name()=$member) and (@name=$name) and (@access=$access) and (not(@declaringType)) and (($contract='Static' and @contract='Static') or ($contract!='Static' and @contract!='Static'))]">
						<td width="50%">
							<xsl:call-template name="images">
								<xsl:with-param name="access" select="@access" />
								<xsl:with-param name="contract" select="@contract" />
								<xsl:with-param name="local-name" select="local-name()" />
							</xsl:call-template>
							<a href="{NUtil:GetMemberOverloadsHRef( string( ../@id ), string( @name ) )}">
								<xsl:choose>
									<xsl:when test="local-name()='operator'">
										<xsl:call-template name="operator-name">
											<xsl:with-param name="name" select="@name" />
											<xsl:with-param name="from" select="parameter/@type"/>
											<xsl:with-param name="to" select="@returnType" />
										</xsl:call-template>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="@name" />
									</xsl:otherwise>
								</xsl:choose>
							</a>
						</td>
						<td width="50%">
							<xsl:text>Overloaded. </xsl:text>
							<xsl:call-template name="overloads-summary-with-no-paragraph" />
						</td>
					</xsl:when>
					<xsl:otherwise>
						<td width="50%">
							<xsl:call-template name="images">
								<xsl:with-param name="access" select="@access" />
								<xsl:with-param name="contract" select="@contract" />
								<xsl:with-param name="local-name" select="local-name()" />
							</xsl:call-template>
							<a href="{NUtil:GetMemberHRef( . )}">
								<xsl:choose>
									<xsl:when test="local-name()='operator'">
										<xsl:call-template name="operator-name">
											<xsl:with-param name="name" select="@name" />
											<xsl:with-param name="from" select="parameter/@type"/>
											<xsl:with-param name="to" select="@returnType" />
										</xsl:call-template>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="@name" />
									</xsl:otherwise>
								</xsl:choose>
							</a>
						</td>
						<td width="50%">
							<xsl:if test="@contract='Override'">
								<xsl:text>Overidden. </xsl:text>
							</xsl:if>
							<xsl:if test="@overload">
								<xsl:text>Overloaded. </xsl:text>
							</xsl:if>
							<xsl:call-template name="summary-with-no-paragraph" />
						</td>
					</xsl:otherwise>
				</xsl:choose>
			</tr>
		</xsl:if>
	</xsl:template>
	<!-- -->
</xsl:stylesheet>
