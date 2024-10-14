-- DB HERE

CREATE SCHEMA calorieCounter;
GO

CREATE TABLE calorieCounter.[Users]
(
    Email NVARCHAR(255) NOT NULL PRIMARY KEY,   -- Corresponds to 'Email' property
    Name NVARCHAR(255) NOT NULL,                -- Corresponds to 'Name' property
);
GO


CREATE TABLE calorieCounter.[Products] (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(100) NOT NULL,
    values_per FLOAT NOT NULL,
    energy FLOAT NOT NULL,
    protein FLOAT NOT NULL,
    carbohydrates FLOAT NOT NULL,
    fat FLOAT NOT NULL,
    owner_email NVARCHAR(255),
    FOREIGN KEY (owner_email) REFERENCES calorieCounter.[Users](Email) ON DELETE CASCADE
);
GO


CREATE TABLE calorieCounter.[Recipes] (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(100) NOT NULL,
    instructions NVARCHAR(MAX) NOT NULL,
    owner_email NVARCHAR(255),
    FOREIGN KEY (owner_email) REFERENCES calorieCounter.[Users](Email) ON DELETE CASCADE
);
GO


CREATE TABLE calorieCounter.[Recipe_Products] (
    recipe_id INT NOT NULL,
    product_id INT NOT NULL,
    weight FLOAT NOT NULL,
    PRIMARY KEY (recipe_id, product_id),
    FOREIGN KEY (recipe_id) REFERENCES calorieCounter.[Recipes](id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES calorieCounter.[Products](id) ON DELETE NO ACTION 
);
GO


CREATE TABLE calorieCounter.[User_Entries] (
    id INT PRIMARY KEY IDENTITY(1,1),
    user_email NVARCHAR(255) NOT NULL,
    entry_type NVARCHAR(10) CHECK (entry_type IN ('product', 'recipe')),
    product_id INT NULL,
    recipe_id INT NULL,
    date DATE NOT NULL,
    meal_type NVARCHAR(20) CHECK (meal_type IN ('Breakfast', 'Lunch', 'Dessert', 'Dinner')),
    weight FLOAT NULL,
    FOREIGN KEY (user_email) REFERENCES calorieCounter.[Users](Email) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES calorieCounter.[Products](id) ON DELETE NO ACTION,
    FOREIGN KEY (recipe_id) REFERENCES calorieCounter.[Recipes](id) ON DELETE NO ACTION
);
GO