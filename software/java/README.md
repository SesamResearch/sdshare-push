## Sample receiver

You are expected to have JDK 7+, Maven 3 and Jetty runner 9.x installed.

Code is compiled with Maven 3. You are supposed to run following command in `sdshare-push/software/java` folder: `mvn clean package`

Replace expressions in angle brackets with actual paths on your system. Replace slashes in case you are running Windows.

It would be possible to run servlet with following command: `java -jar <PATH_TO_JETTY_RUNNER>/jetty-runner.jar --port <PORT_NUMBER> <PATH_TO_REPO>/sdshare-push/software/java/target/sdshare-push-receiver.war`

To post triples to the endpoint:
    
cat file.nt | curl -d "resource=http://psi.test.com/1&graph=http://psi.test.com/test" http://localhost:<PORTNUMBER>/
