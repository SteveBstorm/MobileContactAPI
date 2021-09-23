/*
Modèle de script de post-déploiement							
--------------------------------------------------------------------------------------
 Ce fichier contient des instructions SQL qui seront ajoutées au script de compilation.		
 Utilisez la syntaxe SQLCMD pour inclure un fichier dans le script de post-déploiement.			
 Exemple :      :r .\monfichier.sql								
 Utilisez la syntaxe SQLCMD pour référencer une variable dans le script de post-déploiement.		
 Exemple :      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

INSERT INTO Contact (FirstName, LastName, Email, Telephone, IsFavorite)
VALUES ('Arthur', 'Pendragon', 'arthur@kaamelott.com', '0123/45.67.89', 1)


INSERT INTO Contact (FirstName, LastName, Email, Telephone, IsFavorite)
VALUES ('Perceval', 'De Galles', 'provencal@legaulois.com', '0123/98.67.34', 0)