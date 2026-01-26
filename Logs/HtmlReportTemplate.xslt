<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
		<html>
			<body>
				<h2>Test: <xsl:value-of select="TestRunData/TestName"/></h2>
				<h4>Description: <xsl:value-of select="TestRunData/TestDescription"/></h4>
				<h4>Project: <xsl:value-of select="TestRunData/TestProject"/></h4>
				<h4>Started: 
					<xsl:value-of select="substring-before(TestRunData/StartDateTime, 'T')"/>
					<xsl:text> </xsl:text>
					<xsl:value-of select="substring(TestRunData/StartDateTime, 12, 8)"/>
				</h4>
				<h4>
					Finished:
					<xsl:value-of select="substring-before(TestRunData/FinishDateTime, 'T')"/>
					<xsl:text> </xsl:text>
					<xsl:value-of select="substring(TestRunData/FinishDateTime, 12, 8)"/>
				</h4>
				<h4>Status: 
					<xsl:variable name="status" select="TestRunData/TestStatus"/>
					<xsl:choose>
						<xsl:when test="$status='Passed'">
							<b style="color:green">Passed</b>
						</xsl:when>
						<xsl:when test="$status='Failed'">
							<b style="color:red">Failed</b>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$status" />
						</xsl:otherwise>
					</xsl:choose>
				</h4>
				<table border="2">
					<tr bgcolor="#9acd32">
						<th>Step #</th>
						<th>Status</th>
						<th>Description</th>
						<th>Expected</th>
					</tr>
					<xsl:for-each select="TestRunData/Steps/StepData">
						<tr>
							<td><xsl:value-of select="Number"/></td>
							<td>
								<xsl:variable name="step_status" select="StepStatus"/>
								<xsl:choose>
									<xsl:when test="$step_status='Passed'">
										<b style="color:green">Passed</b>
									</xsl:when>
									<xsl:when test="$step_status='Failed'">
										<b style="color:red">Failed</b>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$step_status" />
									</xsl:otherwise>
								</xsl:choose>
							</td>
							
							<td><xsl:value-of select="Description"/></td>
							<td><xsl:value-of select="Expected"/>
								<xsl:for-each select="Messages/MessageData">
										<xsl:choose>
											<xsl:when test="MessageType = 'Error'">							
												<br><b style="color:red"><xsl:value-of select="MessageType"/>: </b>
												<xsl:value-of select="MessageText"/></br>
											</xsl:when>
											<xsl:when test="MessageType = 'Info'">
												<br><b style="color:green"><xsl:value-of select="MessageType"/>: </b>
												<xsl:value-of select="MessageText"/></br>
											</xsl:when>
											<xsl:when test="MessageType = 'Screenshot'">
												<form style="margin:0px">
													<xsl:attribute name="action">
														<xsl:value-of select="MessageText"/>
													</xsl:attribute>
													<input type="submit" value="Screenshot"/>
												</form>
											</xsl:when>
											<xsl:when test="MessageType = 'Exception'">
												<br><b style="color:red"><xsl:value-of select="MessageType"/>: </b>
													<xsl:value-of select="MessageText"/>
												</br>
											</xsl:when>
										</xsl:choose>
								</xsl:for-each>
							</td>
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>