## Sample receiver

You are expected to have JDK 7+, Maven 3 and [Jetty runner 9.x](http://search.maven.org/#artifactdetails|org.eclipse.jetty|jetty-runner|9.2.4.v20141103|jar) installed.

Code is compiled with Maven 3. You are supposed to run following command in `sdshare-push/software/java` folder: `mvn clean package`

Replace expressions in angle brackets with actual paths on your system. Replace slashes in case you are running Windows.

It would be possible to run servlet with following command: `java -jar <PATH_TO_JETTY_RUNNER>/jetty-runner.jar --port <PORT_NUMBER> <PATH_TO_REPO>/sdshare-push/software/java/target/sdshare-push-receiver.war`

To post triples to the endpoint (is expected to be executed from the root folder of the repository, `sdshare-push`): `curl -X POST 'http://localhost:<PORT_NUMBER>/?graph=<GRAPH_NAME>&resource=<RESOURCE_NAME>' -H 'Content-type: <CONTENT_TYPE>' --data-binary @software/python/SDSharePushReceiver/sdsharepushreceiver/testdata/test.nt`