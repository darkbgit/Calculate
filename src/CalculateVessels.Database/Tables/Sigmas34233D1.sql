CREATE TABLE [dbo].[Sigmas34233D1]
(
    [SteelId] int NOT NULL, 
    [T] FLOAT NOT NULL, 
    [SigmaAllow] FLOAT NOT NULL, 
    [MinMaxThicknessId] INT NULL, 
    [DesignResourceId] INT NULL, 
    CONSTRAINT [FK_Sigmas34233D1_Steels] FOREIGN KEY ([SteelId]) REFERENCES [Steels]([Id]), 
    CONSTRAINT [FK_Sigmas34233D1_DesignResources] FOREIGN KEY ([DesignResourceId]) REFERENCES [DesignResources]([Id]),
)
GO
