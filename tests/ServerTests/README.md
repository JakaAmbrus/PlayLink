# Server Testing

Comprehensive testing is conducted for my API to ensure the reliability and robustness of the application. The automated testing is divided into two main categories: **Unit Testing** and **Integration Testing**. While developing the application, I also conducted **Manual Testing** to validate the application's functionality and usability.

## Application Unit Testing

The unit tests focus on testing the services, utility functions, and handlers of every single feature in the application layer. Here are the key aspects:

- **In-Memory Database:** An in-memory database is used to simulate database operations, enabling isolated testing of data-related logic.
- **Comprehensive Coverage:** Approximately 300 unit tests cover various scenarios, ensuring that each component of the application layer behaves as expected under different conditions.
- **Testing Tools:** `XUnit` for the testing framework, `NSubstitute` for creating test doubles, and `Fluent Assertions` for more readable assertion syntax.

## WebAPI Integration Testing

(Ongoing as of the README's last update)

Integration tests are designed to test the controller endpoints and the overall integration of the various components of the application:

- **Docker PostgreSQL Test Database:** Each test spins up a Docker PostgreSQL test database, which is reset after every test to ensure test isolation and consistency.
- **Sequential Testing:** Tests are run sequentially to avoid conflicts and interdependencies between tests.
- **Authorization Simulation:** Custom logic is implemented to attach the appropriate authorized user header in tests, allowing them to bypass security checks for testing purposes.
- **Data Seeding:** Each test seeds the data it requires, ensuring that tests have the necessary environment set up that reflects the real-world application.
- **Endpoint Response Validation:** `System.Net.Http.Json` is used for testing endpoint responses, along with custom classes for specific response formats like pagination and error exceptions.
- **Testing Tools:** `XUnit` for the testing framework, `Fluent Assertions` for more readable assertion syntax,`NSubstitute` for mocking Cloudinary and `System.Net.Http.Json` for testing endpoint responses.

### Integration Testing Local Setup

To successfully run the integration tests in the `webAPI.tests.integration` suite, it is essential to have Docker installed on your system. The integration tests rely on Docker to spin up a PostgreSQL test database for each test, which ensures a consistent and isolated testing environment. This approach allows the tests to operate in a controlled setting, mimicking real-world database interactions without affecting a live database.

1. **Install Docker:** Ensure that Docker is installed and running on your machine. Docker is used to create a containerized PostgreSQL database, which is essential for the integration tests.
2. **Check Docker Settings:** Verify that Docker is properly configured and has sufficient resources allocated. This is crucial for the smooth creation and operation of the test database containers.
3. **Run Integration Tests:** With Docker running, you can proceed to execute the integration tests. The test suite will automatically handle the creation and disposal of the PostgreSQL database container for each test.

By following these steps, you can ensure that the integration tests run effectively, providing reliable results for your application's integrated components.

For more information on installing and configuring Docker, please refer to the [official Docker documentation](https://docs.docker.com/get-docker/).

## Manual Testing

In addition to automated unit and integration tests, manual testing played a crucial role in the development of the PlayLink application. This approach provided an additional layer of validation to ensure the application's functionality and usability. The following tools were instrumental in this process:

- **Swagger:** Used for testing and interacting with the WebAPI. Swagger offers a user-friendly interface to manually execute API calls and inspect their responses, making it an invaluable tool for testing and documenting the API endpoints.
- **Postman:** A crucial tool for more comprehensive and customizable testing of the API. Postman was used to simulate client requests, test different scenarios, and validate the behavior of API endpoints, including those requiring authentication and authorization. I have grown quite efficient with Postman and have used it extensively throughout the development of the PlayLink application.
- **DBeaver:** Employed for direct database interaction and management. DBeaver allowed for manual inspection, querying, and manipulation of the PostgreSQL database, providing insights into how the database responds to various operations performed by the application. This was especially useful in the early stages when I was still in the process of learning the Entity Framework Core and relational databases in general.
