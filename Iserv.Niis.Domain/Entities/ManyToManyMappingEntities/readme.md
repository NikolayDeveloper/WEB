https://docs.microsoft.com/en-us/ef/core/modeling/relationships

Many-to-many relationships without an entity class to represent the join table are not yet supported. 
However, you can represent a many-to-many relationship by including an entity class for the join table and mapping two separate one-to-many relationships.