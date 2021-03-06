USE [SEGURIMAX02]
GO
/****** Object:  StoredProcedure [dbo].[Actualizar_Rpta_Cdr_GR]    Script Date: 15/03/2021 21:00:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Actualizar_Rpta_Cdr_GR]
@Docentry int,
@U_ResponseCode varchar(2),
@U_Description varchar(50),
@U_DigestValue varchar(250)
AS
BEGIN

UPDATE ODLN	
SET U_ResponseCode = @U_ResponseCode, U_Description = @U_Description, U_DigestValue = @U_DigestValue
WHERE DocEntry = @Docentry
END
GO
/****** Object:  StoredProcedure [dbo].[Consulta_Datos_Generales]    Script Date: 15/03/2021 21:00:21 ******/
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
/****** Object:  StoredProcedure [dbo].[Consulta_Datos_Reporte_GR]    Script Date: 15/03/2021 21:00:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Consulta_Datos_Reporte_GR]
@DocEntry int
AS
BEGIN
	SELECT T0.RevOffice AS RucEmi,T0.CompnyName as NomEmi,T0.CompnyAddr as DirEmi,T1.ZipCode,
		T2.LicTradNum,T2.CardName,'VENTA' AS MotTras,t2.Address2,
		CONVERT(VARCHAR,t2.DocDate,103)AS DocDate,CONVERT(VARCHAR,t2.TaxDate,103) AS TaxDate,'Transporte privado' as'TransPrivado',
		'AES-390' as PlaTra, '46757706' as NumLic, 'Juan C.' as NomCon,t2.U_DigestValue,t4.SeriesName+' - '+convert(varchar,t2.FolioNum) as NumCor,
		/*DETALLE*/
		T3.ItemCode,CONVERT(INT,T3.Quantity)AS Quantity,T3.unitMsr,T3.Dscription

	FROM OADM T0
	CROSS JOIN ADM1 T1
	CROSS JOIN ODLN T2 
	LEFT JOIN DLN1 T3 ON T2.DocEntry = T3.DocEntry
	LEFT JOIN NNM1 T4 ON T2.Series = T4.Series

	WHERE T2.DocEntry = @DocEntry

END
GO
/****** Object:  StoredProcedure [dbo].[Consulta_GuiasRemision]    Script Date: 15/03/2021 21:00:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Consulta_GuiasRemision]
	@FECINI DATE,
	@FECFIN DATE
AS
BEGIN

SELECT DocEntry,CONVERT(varchar,T0.DocDate,103),LicTradNum AS 'RUC',CardName AS 'RAZóN SOCIAL',T1.SeriesName AS 'SERIE', T0.FolioNum AS 'NUM. CORRE.',U_ResponseCode AS 'COD SUNAT',U_Description AS 'RES. SUNAT'  
FROM ODLN t0
	LEFT JOIN NNM1 T1 ON T0.Series = T1.Series
	WHERE T0.DocDate BETWEEN @FECINI AND @FECFIN
END
GO
/****** Object:  StoredProcedure [dbo].[Consulta_SFS_CAB_GR]    Script Date: 15/03/2021 21:00:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Consulta_SFS_CAB_GR]
@DocEntry int
AS
BEGIN
	SELECT CONVERT(varchar,CONVERT(DATE,T0.DocDate)),
		CASE
			WHEN LEN(T0.DocTime)=3
				THEN CONVERT(VARCHAR,LEFT('0'+CONVERT(VARCHAR,T0.DocTime),2)+':'+RIGHT(T0.DocTime,2)+':'+'00')
				ELSE CASE
				WHEN LEN(T0.DocTime) = 2
				THEN CONVERT(VARCHAR,'00'+':'+RIGHT(T0.DocTime,2)+':'+'00')
				ELSE CONVERT(varchar,LEFT(T0.DocTime,2)+':'+RIGHT(T0.DocTime,2)+':'+'00')
		END
			END,
			'09',T1.SeriesName+'-'+CONVERT(VARCHAR,T0.FolioNum),T0.LicTradNum,'6',T0.CardName,
			'01','VENTA','false','0.0','KGM','','02',
			CONVERT(varchar,CONVERT(DATE,T0.DocDate)),'','','','AES-390','46757706','1','RELY',
			T2.ZipCodeS,T0.Address2,
			(SELECT TOP 1 ZipCode FROM ADM1),(SELECT TOP 1 Street FROM ADM1),
			'2.1','1.0'

FROM ODLN T0 
	LEFT JOIN NNM1 T1 ON T0.Series = T1.Series
	LEFT JOIN DLN12 T2 ON T0.DocEntry = T2.DocEntry
WHERE T0.DocEntry = @DocEntry
END
GO
/****** Object:  StoredProcedure [dbo].[Consulta_SFS_DET_GR]    Script Date: 15/03/2021 21:00:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Consulta_SFS_DET_GR]
@DocEntry int
AS
BEGIN
	SELECT 'NIU', CONVERT(numeric(18,2),Quantity),Dscription,ItemCode FROM DLN1

WHERE DocEntry = @DocEntry
END
GO
/****** Object:  StoredProcedure [dbo].[SBO_SP_PostTransactionNotice]    Script Date: 15/03/2021 21:00:21 ******/
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

	/*gr*/
	IF @object_type = '15' AND @transaction_type ='A'
	begin
		declare @FoliNum2 int
		set @FoliNum2 =(select MAX(FolioNum) from ODLN)
		update ODLN
		set 
			FolioPref = 'G1',
			FolioNum = @FoliNum2+1,
			Printed = 'Y'
		where DocEntry = @list_of_cols_val_tab_del
	end
--------------------------------------------------------------------------------------------------------------------------------

-- Select the return values
select @error, @error_message

end
GO
