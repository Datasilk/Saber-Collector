﻿BEGIN TRY
	CREATE SEQUENCE [dbo].[SequenceDomainTypeMatches]
		AS BIGINT
		START WITH 1
		INCREMENT BY 1
		NO MAXVALUE
		NO CYCLE
		NO CACHE
END TRY BEGIN CATCH END CATCH