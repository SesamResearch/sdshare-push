package no.bouvet.sesam.sdshare;

import java.io.BufferedReader;


/**
 * Created by petr.vasilev on 10.11.14.
 */
// represents the parameters of an incoming HTTP request
public class SDSRequest {
    private String contentType;
    private String graph;
    private String resource;
    private BufferedReader ntriples;

    public String getContentType() {
        return contentType;
    }

    public String getGraph() {
        return graph;
    }

    public String getResource() {
        return resource;
    }

    public BufferedReader getNtriples() {
        return ntriples;
    }

    public SDSRequest(String graph, String resource, String contentType, BufferedReader ntriples) {
        this.graph = graph;
        this.resource = resource;
        this.ntriples = ntriples;
        this.contentType = contentType;
    }

}
