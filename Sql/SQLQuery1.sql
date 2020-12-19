USE [SEGURIMAX02]
GO
/****** Object:  StoredProcedure [dbo].[Actualizar_Rpta_Cdr]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Actualizar_Rpta_Cdr]
@Docentry int,
@U_ResponseCode varchar(2),
@U_Description varchar(50)
AS
BEGIN

UPDATE OINV	
SET U_ResponseCode = @U_ResponseCode, U_Description = @U_Description
WHERE DocEntry = @Docentry
END
GO
/****** Object:  StoredProcedure [dbo].[Actualizar_Rpta_Cdr_NC]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Actualizar_Rpta_Cdr_NC]
@Docentry int,
@U_ResponseCode varchar(2),
@U_Description varchar(50)
AS
BEGIN

UPDATE ORIN	
SET U_ResponseCode = @U_ResponseCode, U_Description = @U_Description
WHERE DocEntry = @Docentry
END
GO
/****** Object:  StoredProcedure [dbo].[Consulta_Datos_Generales]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Consulta_Datos_Generales]
	
AS
BEGIN
DECLARE @ZIPCODE VARCHAR(10)
SET @ZIPCODE = (SELECT ZipCode FROM ADM1)

SELECT RevOffice,CompnyName,CompnyAddr,@ZIPCODE FROM OADM-- // 20603346735 |	DyE	| direccion |	180101
END
GO
/****** Object:  StoredProcedure [dbo].[Consulta_DatosCorreo]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Consulta_DatosCorreo]
@DocEntry int
AS
BEGIN
DECLARE @CompnyName VARCHAR(100)
DECLARE @RevOffice VARCHAR(11)
DECLARE @DocNum1 INT


SET @CompnyName = (SELECT CompnyName FROM OADM)
SET @RevOffice = (SELECT RevOffice FROM OADM)
--SET @DocNum1 = @DocNum

--IF @DocNum1 >10000
--BEGIN
--SET @DocNum1 = @DocNum1-10000
--END


SELECT @CompnyName, @RevOffice,
T1.SeriesName+'-'+RIGHT('00000' + Ltrim(Rtrim(T0.FolioNum)),4),

T0.CardName,t2.E_Mail, CONVERT(varchar,T0.DocDate,103)
		,CONVERT(varchar,CONVERT(MONEY,T0.DocTotal))
FROM OINV T0 INNER JOIN NNM1 T1 ON T0.Series = T1.Series INNER JOIN OCRD t2 ON t0.CardCode = t2.CardCode 
where t0.DocEntry = @DocEntry
END
GO
/****** Object:  StoredProcedure [dbo].[Consulta_factura_DetalleCR]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Consulta_factura_DetalleCR]

@DocNum int
AS
BEGIN


SELECT DocEntry,Quantity,unitMsr,Dscription,CONVERT(VARCHAR,CONVERT(MONEY,Price))AS'Price',CONVERT(VARCHAR,CONVERT(MONEY,PriceAfVAT))AS'PriceAfVAT',CONVERT(VARCHAR,CONVERT(MONEY,GTotal))AS'GTotal' FROM INV1 WHERE DocEntry =(select DocEntry from OINV where DocNum = @DocNum)
END
GO
/****** Object:  StoredProcedure [dbo].[Consulta_factura_EncabezadoCR]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Consulta_factura_EncabezadoCR]
	@DocNum int
	
AS
BEGIN
	
DECLARE @RUCDG VARCHAR(11)
DECLARE @NOMBREDG VARCHAR (100)
DECLARE @DIRECCION VARCHAR(100)
DECLARE @DATOS VARCHAR(200)
DECLARE @DocNum1 int


SET @RUCDG = (SELECT RevOffice FROM OADM)
SET @NOMBREDG = (SELECT CompnyName FROM OADM) 
SET @DIRECCION = (SELECT CompnyAddr FROM OADM)
SET @DATOS = 'Ricardo Fernandez F. CEL: 970066001  ventas@rybseguridad.com'--AQUI SE PUEDE COLOCAR DATOS COMO TELEFONOS, BREVE DESCRIPCION DE NEGOCIO
IF @DocNum >10000
BEGIN
SET @DocNum1 = @DocNum-10000 --PARA EL CASO DE LAS BOLETAS RESTAR LOS 10000 DEL SOLAPAMIENTO
END
ELSE
BEGIN
SET @DocNum1 = @DocNum
END



SELECT @RUCDG AS'RevOffice', @NOMBREDG AS 'CompnyName',@DIRECCION AS'CompnyAddr',@DATOS AS 'DATOS' 
		,T1.SeriesName,RIGHT('00000' + Ltrim(Rtrim(T0.FolioNum)),4)AS'DocNum'
		,T0.LicTradNum,T0.CardName,T0.Address,CONVERT(varchar,T0.DocDate,103)AS'DocDate',CONVERT(varchar,T0.DocDueDate,103)AS'DocDueDate'
		,CONVERT(varchar,CONVERT(money,T0.DocTotal-T0.VatSum)) AS'GrosProfit',CONVERT(varchar,CONVERT(money,T0.VatSum)) AS'VatSum',CONVERT(VARCHAR,CONVERT(money,T0.DocTotal)) AS'DocTotal'
		, (Select dbo.CantidadConLetra(T0.DocTotal))AS'DocTotalLetras',''as'CodHast'

FROM OINV T0 INNER JOIN NNM1 T1 ON T0.Series = T1.Series WHERE T0.DocNum = @DocNum


END


GO
/****** Object:  StoredProcedure [dbo].[Consulta_SFS_ACA]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Consulta_SFS_ACA]
	@DocEntry INT

AS
BEGIN
SELECT '-','-','-','-',T1.PymntGroup
		,'PE','010101',T0.Address,'-','-'
		,'-'
FROM OINV T0 INNER JOIN OCTG T1 ON T0.GroupNum = T1.GroupNum WHERE T0.DocEntry =@DocEntry
END
GO
/****** Object:  StoredProcedure [dbo].[Consulta_SFS_ACA_NC]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Consulta_SFS_ACA_NC]
	@DocEntry INT

AS
BEGIN
SELECT '-','-','-','-',T1.PymntGroup
		,'PE','010101',T0.Address,'-','-'
		,'-'
FROM ORIN T0 INNER JOIN OCTG T1 ON T0.GroupNum = T1.GroupNum WHERE T0.DocEntry =@DocEntry
END
GO
/****** Object:  StoredProcedure [dbo].[Consulta_SFS_CAB]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Consulta_SFS_CAB]
	@DocEntry INT
AS
BEGIN
	DECLARE @UBIGEO VARCHAR(10)
SET @UBIGEO = (SELECT ZipCode FROM ADM1)



SELECT '0101',CONVERT(varchar,CONVERT(DATE,T0.TaxDate)), 

--CONVERT(varchar,LEFT(T0.DocTime,2)+':'+RIGHT(T0.DocTime,2)+':'+'00')
CASE
	WHEN LEN(T0.DocTime)=3
		THEN CONVERT(VARCHAR,LEFT('0'+CONVERT(VARCHAR,T0.DocTime),2)+':'+RIGHT(T0.DocTime,2)+':'+'00')
		ELSE CONVERT(varchar,LEFT(T0.DocTime,2)+':'+RIGHT(T0.DocTime,2)+':'+'00')
	END

,CONVERT(VARCHAR,CONVERT(DATE,T0.DOCDUEDATE)),'0000',
		CASE
			WHEN LEFT(T0.Series,1) = 'B'
				THEN '1'
				ELSE '6'
			END
			
			--,T0.LicTradNum,T0.CardName,'PEN',CONVERT(money,T0.VatSum) funcionando
			,T0.LicTradNum,T0.CardName,'PEN',CONVERT(numeric(18,2),round(((T0.DocTotal/1.18)*.18),2))
			,CONVERT(numeric(18,2),round(T0.DocTotal/1.18,2)),CONVERT(numeric(12,2),T0.DocTotal),'0.00','0.00','0.00'
			,CONVERT(numeric(12,2),T0.DocTotal),'2.1','2.0'
FROM OINV T0 WHERE T0.DocEntry =@DocEntry
END
GO
/****** Object:  StoredProcedure [dbo].[Consulta_SFS_CAB_NC]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Consulta_SFS_CAB_NC]
	@DocEntry INT
AS
BEGIN
	DECLARE @DocEntryFac int
DECLARE @SerieNumFac varchar(20)

SET @DocEntryFac = (SELECT top 1 BaseEntry FROM RIN1 WHERE DocEntry = @DocEntry)

SET @SerieNumFac = (SELECT T1.SeriesName+'-'+convert(varchar,T0.FolioNum) FROM OINV T0 INNER JOIN NNM1 T1 ON T0.Series = T1.Series WHERE T0.DocEntry = @DocEntryFac)


SELECT '0101',CONVERT(varchar,CONVERT(DATE,T0.TaxDate)), 

--CONVERT(varchar,LEFT(T0.DocTime,2)+':'+RIGHT(T0.DocTime,2)+':'+'00')
CASE
	WHEN LEN(T0.DocTime)=3
		THEN CONVERT(VARCHAR,LEFT('0'+CONVERT(VARCHAR,T0.DocTime),2)+':'+RIGHT(T0.DocTime,2)+':'+'00')
		ELSE CONVERT(varchar,LEFT(T0.DocTime,2)+':'+RIGHT(T0.DocTime,2)+':'+'00')
	END,'0000',
		CASE
			WHEN LEFT(T0.Series,1) = 'B'
				THEN '1'
				ELSE '6'
			END
			AS'5'
			--,T0.LicTradNum,T0.CardName,'PEN',CONVERT(money,T0.VatSum) funcionando
			,T0.LicTradNum,T0.CardName,'PEN',
			T0.U_TipNotCre,t1.Name as '10',
			'01',@SerieNumFac,
			
			CONVERT(numeric(18,2),round(((T0.DocTotal/1.18)*.18),2))
			,CONVERT(numeric(18,2),round(T0.DocTotal/1.18,2)),CONVERT(numeric(12,2),T0.DocTotal) as'15','0.00','0.00','0.00'
			,CONVERT(numeric(12,2),T0.DocTotal),'2.1','2.0'
FROM ORIN T0 
LEFT JOIN dbo.[@TIPNOCR] T1 ON T0.U_TipNotCre = T1.Code
LEFT JOIN RIN1 T2 ON T0.DocEntry = T2.DocEntry 

WHERE T0.DocEntry =@DocEntry
END
GO
/****** Object:  StoredProcedure [dbo].[Consulta_SFS_DET]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Consulta_SFS_DET]
	@DocEntry INT
AS
BEGIN


SELECT CASE 
			WHEN T0.unitMsr = 'PAR'
				THEN 'NIU'
				ELSE 'NIU'
			END
			/*
			,CONVERT(varchar,CONVERT(NUMERIC(18,2),T0.Quantity)),T0.ItemCode,'-',T0.Dscription AS '5'
			,CONVERT(varchar,CONVERT(money,T0.Price)),CONVERT(money,T0.VatSum),'1000',CONVERT(varchar,CONVERT(money,T0.VatSum/T0.Quantity)),CONVERT(varchar,CONVERT(money,T0.Price)) AS '10'
			,'IGV','VAT','10',CONVERT(NUMERIC(18,2),T0.VatPrcnt),'-'
			,'0.00','0.00','','',''AS'20'
			,'','-','0.00','0.00',''AS'25'
			,'','','-','0.00',''AS'30'
			,'ICBPER','OTH','0.00',CONVERT(varchar,CONVERT(money,(T0.Price*T0.Quantity))),CONVERT(money,CONVERT(money,(T0.PriceAfVAT*T0.Quantity)))
			,'0.00'
			*/
			
			,CONVERT(varchar,CONVERT(NUMERIC(18,2),T0.Quantity)),T0.ItemCode,'-',T0.Dscription AS '5'
			,CONVERT(varchar,CONVERT(numeric(12,2),T0.Price)),CONVERT(numeric(12,2),T0.VatSum),'1000',CONVERT(VARCHAR,CONVERT(money,T0.VatSum)),CONVERT(varchar,convert(numeric(12,2),t0.LineTotal)) AS '10'
			,'IGV','VAT','10',CONVERT(NUMERIC(18,2),T0.VatPrcnt),'-'
			,'0.00','0.00','','',''AS'20'
			,'0','-','0.00','0.00','OTROS'AS'25'
			,'OTH','0','-','0',''AS'30'
			,'','','',CONVERT(numeric(12,2),CONVERT(numeric(12,2),(T0.PriceAfVAT))),CONVERT(varchar,CONVERT(numeric(12,2),(t0.LineTotal)))
			,'0.00'
		
FROM INV1 T0  INNER JOIN OITM T1 ON T0.ItemCode = T1.ItemCode WHERE T0.DocEntry =@DocEntry

END
GO
/****** Object:  StoredProcedure [dbo].[Consulta_SFS_DET_NC]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Consulta_SFS_DET_NC]
	@DocEntry INT
AS
BEGIN


SELECT CASE 
			WHEN T0.unitMsr = 'PAR'
				THEN 'NIU'
				ELSE 'NIU'
			END
			/*
			,CONVERT(varchar,CONVERT(NUMERIC(18,2),T0.Quantity)),T0.ItemCode,'-',T0.Dscription AS '5'
			,CONVERT(varchar,CONVERT(money,T0.Price)),CONVERT(money,T0.VatSum),'1000',CONVERT(varchar,CONVERT(money,T0.VatSum/T0.Quantity)),CONVERT(varchar,CONVERT(money,T0.Price)) AS '10'
			,'IGV','VAT','10',CONVERT(NUMERIC(18,2),T0.VatPrcnt),'-'
			,'0.00','0.00','','',''AS'20'
			,'','-','0.00','0.00',''AS'25'
			,'','','-','0.00',''AS'30'
			,'ICBPER','OTH','0.00',CONVERT(varchar,CONVERT(money,(T0.Price*T0.Quantity))),CONVERT(money,CONVERT(money,(T0.PriceAfVAT*T0.Quantity)))
			,'0.00'
			*/
			
			,CONVERT(varchar,CONVERT(NUMERIC(18,2),T0.Quantity)),T0.ItemCode,'-',T0.Dscription AS '5'
			,CONVERT(varchar,CONVERT(numeric(12,2),T0.Price)),CONVERT(numeric(12,2),T0.VatSum),'1000',CONVERT(VARCHAR,CONVERT(money,T0.VatSum)),CONVERT(varchar,convert(numeric(12,2),t0.LineTotal)) AS '10'
			,'IGV','VAT','10',CONVERT(NUMERIC(18,2),T0.VatPrcnt),'-'
			,'0.00','0.00','','',''AS'20'
			,'0','-','0.00','0.00','OTROS'AS'25'
			,'OTH','0','-','0',''AS'30'
			,'','','',CONVERT(numeric(12,2),CONVERT(numeric(12,2),(T0.PriceAfVAT))),CONVERT(varchar,CONVERT(numeric(12,2),(t0.LineTotal)))
			,'0.00'
		
FROM RIN1 T0  INNER JOIN OITM T1 ON T0.ItemCode = T1.ItemCode WHERE T0.DocEntry =@DocEntry

END
GO
/****** Object:  StoredProcedure [dbo].[Consulta_SFS_LEY]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Consulta_SFS_LEY]
	@DocEntry INT
AS
BEGIN
SELECT '1000',dbo.CantidadConLetra(CONVERT(money,T0.DocTotal))
FROM OINV T0 WHERE T0.DocEntry =@DocEntry
END
GO
/****** Object:  StoredProcedure [dbo].[Consulta_SFS_LEY_NC]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Consulta_SFS_LEY_NC]
	@DocEntry INT
AS
BEGIN
SELECT '1000',dbo.CantidadConLetra(CONVERT(money,T0.DocTotal))
FROM ORIN T0 WHERE T0.DocEntry =@DocEntry
END
GO
/****** Object:  StoredProcedure [dbo].[Consulta_SFS_TRY]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Consulta_SFS_TRY]
	@DocEntry INT
AS
BEGIN
SELECT '1000','IGV','VAT',CONVERT(numeric(12,2),T0.DocTotal -T0.VatSum),CONVERT(numeric(12,2),T0.VatSum)
--SELECT '1000','IGV','VAT',CONVERT(numeric(18,2),(ROUND(T0.DocTotal /1.18,2))),CONVERT(numeric(18,2),round(((T0.DocTotal /1.18)*.18),2))
FROM OINV T0 WHERE T0.DocEntry =@DocEntry
END
GO
/****** Object:  StoredProcedure [dbo].[Consulta_SFS_TRY_NC]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Consulta_SFS_TRY_NC]
	@DocEntry INT
AS
BEGIN
SELECT '1000','IGV','VAT',CONVERT(numeric(12,2),T0.DocTotal -T0.VatSum),CONVERT(numeric(12,2),T0.VatSum)
--SELECT '1000','IGV','VAT',CONVERT(numeric(18,2),(ROUND(T0.DocTotal /1.18,2))),CONVERT(numeric(18,2),round(((T0.DocTotal /1.18)*.18),2))
FROM ORIN T0 WHERE T0.DocEntry =@DocEntry
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Tipo_NC]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_Tipo_NC]
@DocEntry int
AS
BEGIN
SELECT T0.U_TipNotCre,t1.Name 
	FROM ORIN T0
	LEFT JOIN dbo.[@TIPNOCR] T1 ON T0.U_TipNotCre = T1.Code
	WHERE T0.DocEntry = @DocEntry
END
GO
/****** Object:  StoredProcedure [dbo].[SBO_SP_PostTransactionNotice]    Script Date: 18/12/2020 22:22:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[SBO_SP_PostTransactionNotice]

@object_type nvarchar(20), 				-- SBO Object Type
@transaction_type nchar(1),			-- [A]dd, [U]pdate, [D]elete, [C]ancel, C[L]ose
@num_of_cols_in_key int,
@list_of_key_cols_tab_del nvarchar(255),
@list_of_cols_val_tab_del nvarchar(255)

AS

begin

-- Return values
declare @error  int				-- Result (0 for no error)
declare @error_message nvarchar (200) 		-- Error string to be displayed
select @error = 0
select @error_message = N'Ok'

--------------------------------------------------------------------------------------------------------------------------------

--	ADD	YOUR	CODE	HERE

IF @object_type = '13' AND @transaction_type ='A'
begin
	declare @FoliNum int
	set @FoliNum =(select MAX(FolioNum) from OINV)
	update OINV
	set 
FolioPref = 'F3',
FolioNum = @FoliNum+1,
Printed = 'Y'
where DocEntry = @list_of_cols_val_tab_del
end

IF @object_type = '14' AND @transaction_type ='A'
	begin
		declare @FoliNum1 int
		set @FoliNum1 =(select MAX(FolioNum) from ORIN)
		update ORIN
		set 
			FolioPref = 'N1',
			FolioNum = @FoliNum1+1,
			Printed = 'Y'
		where DocEntry = @list_of_cols_val_tab_del
	end
--------------------------------------------------------------------------------------------------------------------------------

-- Select the return values
select @error, @error_message

end
GO
