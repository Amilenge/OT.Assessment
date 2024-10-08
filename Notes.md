### 2. Running the Application
- Run SQL script located in **Solution Items\DatabaseGenerate.sql** which will create the DB and tables
- Open **two instances of Visual Studio**.
- In the first instance, select the **multiple startup projects** option, and choose the **Api** and **Consumer** projects. Run them together.
- In the second instance, run the **Tester** project.

### 3. Validation
Once all components are running, verify the database for new records. On a local setup, the application is successfully able to process **all 7000 requests**.

## Key Components & Technologies

### 1. **EF Core (Database First Approach)**
- The project uses **Entity Framework Core** with the **Database First** approach.
- Designing the complete database, from start to finish, with appropriate indexing can be challenging with Code First. Hence, Database First ensures we have better control over the design, especially for indexing and other database-specific optimizations.

### 2. **Autofac**
- **Autofac** is used for **dependency injection**. It helps to decouple class responsibilities by separating the creation and management of dependencies from the core business logic.
- This enhances the maintainability and scalability of the solution.

### 3. **RabbitMQ**
- Transitioning from **NServiceBus** to **RabbitMQ** presented some challenges, but RabbitMQ was chosen for its flexibility and efficiency in handling messaging in distributed systems.

### 4. **AutoMapper**
- **AutoMapper** simplifies object-to-object mapping by defining simple mapping rules. This significantly reduces the need for manual mapping logic, improving development speed and code cleanliness.

### 5. **Clean and Domain-Driven Architecture**
- The entire solution has been **re-organized** to follow the principles of **Clean Architecture** and **Domain-Driven Design (DDD)**. This ensures that the business logic is kept separate from infrastructure concerns, promoting a more maintainable and testable solution structure.

---

## Summary
This project leverages key technologies such as **EF Core**, **Autofac**, **RabbitMQ**, and **AutoMapper** to create a scalable and maintainable application. With a well-structured domain architecture and dependency management, the solution is built to handle complex operations efficiently, while ensuring clear separation of concerns.
