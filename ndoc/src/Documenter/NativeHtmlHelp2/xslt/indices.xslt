<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:MSHelp="http://msdn.microsoft.com/mshelp">
<!-- good for debugging -->
<xsl:output indent="yes"/>
	<!-- provide no-op override for all non-specified types -->
	<xsl:template match="* | node() | text()" mode="FIndex"/>
	<xsl:template match="* | node() | text()" mode="KIndex"/>
	<xsl:template match="* | node() | text()" mode="AIndex"/>
	
	<!-- this is just here until each type has it's own title logic -->
	<xsl:template match="* | node() | text()" mode="MSHelpTitle">
		<MSHelp:TOCTitle Title="{@id}"/>
		<MSHelp:RLTitle Title="{@id}"/>
	</xsl:template>
	
	<xsl:template match="ndoc" mode="MSHelpTitle">
		<xsl:param name="title" />
		<MSHelp:TOCTitle Title="{$title}"/>
		<MSHelp:RLTitle Title="{concat( $title, ' Namespace' )}"/>
	</xsl:template>
	
	<xsl:template match="enumeration | delegate | constructor" mode="MSHelpTitle">
		<xsl:param name="title" />
		<MSHelp:TOCTitle Title="{$title}"/>
		<MSHelp:RLTitle Title="{$title}"/>	
	</xsl:template>
	
	<xsl:template match="field | property | method | event" mode="MSHelpTitle">
		<xsl:param name="title" />
		<MSHelp:TOCTitle Title="{$title}"/>
		<MSHelp:RLTitle Title="{concat( parent::node()/@name, '.', $title )}"/>	
	</xsl:template>	
	
	<xsl:template match="class | interface | structure" mode="MSHelpTitle">
		<xsl:param name="title" />
		<xsl:param name="page-type"/>
		<xsl:choose>
			<xsl:when test="$page-type='type' or $page-type='hierarchy' or $page-type='Members'">
				<MSHelp:TOCTitle Title="{$title}"/>
			</xsl:when>
			<xsl:otherwise>
				<MSHelp:TOCTitle Title="{$page-type}"/>
			</xsl:otherwise>
		</xsl:choose>
		<MSHelp:RLTitle Title="{$title}"/>
	</xsl:template>
		
	<!--
	<xsl:template match="field | property | method | event" mode="MSHelpTitle">
		<xsl:param name="title" />
		<xsl:param name="page-type"/>	

		<MSHelp:TOCTitle Title="{concat( @name, ' ', $page-type )}"/>	
		<MSHelp:RLTitle Title="{$title}"/>			
	</xsl:template>
	-->
	
	<xsl:template match="operator" mode="MSHelpTitle">
		<xsl:param name="title" />
		<xsl:param name="page-type"/>	
		<MSHelp:TOCTitle Title="{$page-type}"/>	
		<MSHelp:RLTitle Title="{$title}"/>
	</xsl:template>
		
		
	<xsl:template match="ndoc" mode="FIndex">
		<xsl:param name="title"/>
		<xsl:call-template name="add-index-term">
			<xsl:with-param name="index">F</xsl:with-param>
			<xsl:with-param name="term" select="$title"/>
		</xsl:call-template>
	</xsl:template>		
	
	<xsl:template match="delegate" mode="FIndex">
		<xsl:call-template name="add-index-term">
			<xsl:with-param name="index">F</xsl:with-param>
			<xsl:with-param name="term" select="substring-after( @id, ':')"/>
		</xsl:call-template>
	</xsl:template>	

	<xsl:template match="enumeration" mode="FIndex">
		<xsl:call-template name="add-index-term">
			<xsl:with-param name="index">F</xsl:with-param>
			<xsl:with-param name="term" select="substring-after( @id, ':')"/>
		</xsl:call-template>
		<xsl:apply-templates select="field" mode="FIndex"/>
	</xsl:template>	

	<xsl:template match="class | structure | interface" mode="FIndex">
		<xsl:param name="title"/>
		<xsl:param name="page-type"/>

		<xsl:if test="$page-type='Members' or $page-type='type'">
			<xsl:call-template name="add-index-term">
				<xsl:with-param name="index">F</xsl:with-param>
				<xsl:with-param name="term" select="substring-after( @id, ':' )"/>
			</xsl:call-template>
		</xsl:if>			
	</xsl:template>	

	<xsl:template match="enumeration/field" mode="FIndex">
		<xsl:call-template name="add-index-term">
			<xsl:with-param name="index">F</xsl:with-param>
			<xsl:with-param name="term" select="substring-after( @id, ':')"/>
		</xsl:call-template>
		<xsl:call-template name="add-index-term">
			<xsl:with-param name="index">F</xsl:with-param>
			<xsl:with-param name="term" select="concat( parent::node()/@name, '.', @name )"/>
		</xsl:call-template>		
	</xsl:template>	
	
	<xsl:template match="constructor" mode="FIndex">
		<xsl:param name="overload-page"/>
		
		<xsl:if test="$overload-page=true() or not(@overload)">
			<xsl:call-template name="add-index-term">
				<xsl:with-param name="index">F</xsl:with-param>
				<xsl:with-param name="term" select="concat( substring-after( parent::node()/@id, ':' ), '.', parent::node()/@name )"/>
			</xsl:call-template>
			<xsl:call-template name="add-index-term">
				<xsl:with-param name="index">F</xsl:with-param>
				<xsl:with-param name="term" select="concat( parent::node()/@name, '.', parent::node()/@name )"/>
			</xsl:call-template>
			<xsl:call-template name="add-index-term">
				<xsl:with-param name="index">F</xsl:with-param>
				<xsl:with-param name="term" select="concat( substring-after( parent::node()/@id, ':' ), '.New' )"/>
			</xsl:call-template>
			<xsl:call-template name="add-index-term">
				<xsl:with-param name="index">F</xsl:with-param>
				<xsl:with-param name="term" select="concat( parent::node()/@name, '.New' )"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>	
	
	<xsl:template match="field | property | method | event" mode="FIndex">
		<xsl:param name="overload-page"/>

		<xsl:if test="$overload-page=true() or not(@overload)">
			<xsl:call-template name="add-index-term">
				<xsl:with-param name="index">F</xsl:with-param>
				<xsl:with-param name="term" select="concat( substring-after( parent::node()/@id, ':' ), '.', @name )"/>
			</xsl:call-template>
			<xsl:call-template name="add-index-term">
				<xsl:with-param name="index">F</xsl:with-param>
				<xsl:with-param name="term" select="concat( parent::node()/@name, '.', @name )"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>		
	
	<xsl:template match="ndoc" mode="KIndex">
		<xsl:param name="title" />
		<xsl:if test="contains( $title, '.' )">
			<xsl:call-template name="add-index-term">
				<xsl:with-param name="index">K</xsl:with-param>
				<xsl:with-param name="term" select="concat( substring-after( $title, '.' ), ' Namespace' )"/>
			</xsl:call-template>		
		</xsl:if>
		<xsl:call-template name="add-index-term">
			<xsl:with-param name="index">K</xsl:with-param>
			<xsl:with-param name="term" select="concat( $title, ' Namespace' )"/>
		</xsl:call-template>		
	</xsl:template>	
	
		
	<xsl:template match="class | interface | structure" mode="KIndex">
		<xsl:param name="title"/>
		<xsl:param name="page-type"/>		
		<xsl:choose>
			<xsl:when test="$page-type='Members'">
				<xsl:call-template name="add-index-term">
					<xsl:with-param name="index">K</xsl:with-param>
					<xsl:with-param name="term" select="concat( @name, ' ', local-name() )"/>
				</xsl:call-template>					
				<xsl:call-template name="add-index-term">
					<xsl:with-param name="index">K</xsl:with-param>
					<xsl:with-param name="term" select="concat( @name, ' ', local-name(), ', all members' )"/>
				</xsl:call-template>					
				<xsl:call-template name="add-index-term">
					<xsl:with-param name="index">K</xsl:with-param>
					<xsl:with-param name="term" select="concat( substring-after( @id, ':' ), ' ', local-name() )"/>
				</xsl:call-template>					
			</xsl:when>
			<xsl:when test="$page-type='Properties'">
				<xsl:call-template name="add-index-term">
					<xsl:with-param name="index">K</xsl:with-param>
					<xsl:with-param name="term" select="concat( @name, ' ', local-name(), ', properties' )"/>
				</xsl:call-template>									
			</xsl:when>
			<xsl:when test="$page-type='Events'">
				<xsl:call-template name="add-index-term">
					<xsl:with-param name="index">K</xsl:with-param>
					<xsl:with-param name="term" select="concat( @name, ' ', local-name(), ', events' )"/>
				</xsl:call-template>									
			</xsl:when>
			<xsl:when test="$page-type='Operators'">
				<xsl:call-template name="add-index-term">
					<xsl:with-param name="index">K</xsl:with-param>
					<xsl:with-param name="term" select="concat( @name, ' ', local-name(), ', operators' )"/>
				</xsl:call-template>									
			</xsl:when>
			<xsl:when test="$page-type='Methods'">
				<xsl:call-template name="add-index-term">
					<xsl:with-param name="index">K</xsl:with-param>
					<xsl:with-param name="term" select="concat( @name, ' ', local-name(), ', methods' )"/>
				</xsl:call-template>									
			</xsl:when>
			<xsl:when test="$page-type='Fields'">
				<xsl:call-template name="add-index-term">
					<xsl:with-param name="index">K</xsl:with-param>
					<xsl:with-param name="term" select="concat( @name, ' ', local-name(), ', fields' )"/>
				</xsl:call-template>									
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="add-index-term">
					<xsl:with-param name="index">K</xsl:with-param>
					<xsl:with-param name="term" select="concat( @name, ' ', local-name(), ', about ', @name, ' ', local-name() )"/>
				</xsl:call-template>					
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>	
	
	<xsl:template match="constructor" mode="KIndex">
		<xsl:param name="overload-page"/>

		<xsl:if test="$overload-page=true() or not(@overload)">
			<xsl:call-template name="add-index-term">
				<xsl:with-param name="index">K</xsl:with-param>
				<xsl:with-param name="term" select="concat( parent::node()/@name, ' ', local-name( parent::node() ), ', ', local-name() )"/>
			</xsl:call-template>				
			<xsl:call-template name="add-index-term">
				<xsl:with-param name="index">K</xsl:with-param>
				<xsl:with-param name="term" select="concat( substring-after( parent::node()/@id, ':' ), ' ', local-name() )"/>
			</xsl:call-template>	
		</xsl:if>			
	</xsl:template>	
	
	<xsl:template match="field | property | method | event" mode="KIndex">
		<xsl:param name="overload-page" />

		<xsl:if test="$overload-page=true() or not(@overload)">
			<xsl:call-template name="add-index-term">
				<xsl:with-param name="index">K</xsl:with-param>
				<xsl:with-param name="term" select="concat( parent::node()/@name, '.', @name, ' ', local-name() )"/>
			</xsl:call-template>				
			<xsl:call-template name="add-index-term">
				<xsl:with-param name="index">K</xsl:with-param>
				<xsl:with-param name="term" select="concat( @name, ' ', local-name() )"/>
			</xsl:call-template>	
		</xsl:if>			
	</xsl:template>	
			
	<xsl:template match="enumeration" mode="KIndex">
		<xsl:apply-templates select="field" mode="KIndex"/>
		<xsl:call-template name="add-index-term">
			<xsl:with-param name="index">K</xsl:with-param>
			<xsl:with-param name="term" select="concat( substring-after( @id, ':'), ' enumeration' )"/>
		</xsl:call-template>		
	</xsl:template>	
	
	<xsl:template match="enumeration/field" mode="KIndex">
		<xsl:call-template name="add-index-term">
			<xsl:with-param name="index">K</xsl:with-param>
			<xsl:with-param name="term" select="concat( @name, ' enumeration member' )"/>
		</xsl:call-template>		
	</xsl:template>
	
		<xsl:template match="delegate" mode="KIndex">
		<xsl:call-template name="add-index-term">
			<xsl:with-param name="index">K</xsl:with-param>
			<xsl:with-param name="term" select="concat( @name, ' delegate' )"/>
		</xsl:call-template>		
		<xsl:call-template name="add-index-term">
			<xsl:with-param name="index">K</xsl:with-param>
			<xsl:with-param name="term" select="concat( substring-after( @id, ':' ), ' delegate' )"/>
		</xsl:call-template>		
	</xsl:template>
	
	
	<xsl:template name="add-index-term">
		<xsl:param name="index"/>
		<xsl:param name="term"/>
		<MSHelp:Keyword Index="{$index}" Term="{$term}"/>
	</xsl:template>	
	
</xsl:stylesheet>

  