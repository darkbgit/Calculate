CREATE TABLE [dbo].[E34233D1]
(
	[SteelTypeId] INT NOT NULL, 
    [T] FLOAT NOT NULL, 
    [E] FLOAT NOT NULL, 
    CONSTRAINT [FK_E34233D1_ToSteelTypes] FOREIGN KEY ([SteelTypeId]) REFERENCES [SteelTypes]([Id]) 
)
