package no.bouvet.sesam.sdshare;

import java.io.BufferedReader;
import java.io.IOException;

import javax.servlet.ServletConfig;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;


import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class PushReceiverServlet extends HttpServlet {

    static Logger log = LoggerFactory.getLogger(PushReceiverServlet.class.getName());

    public PushReceiverServlet() { }

    public void init(ServletConfig config) throws ServletException {

        // FIXME: Add code to read in config files
        log.info("Receiver loaded");
    }

    protected void doPost(HttpServletRequest req, HttpServletResponse resp)
        throws ServletException, IOException {
        SDSRequest request = parseRequest(req);
       // FIXME: Add code to parse incoming SDShare push request and send calls further on

        log.trace("Processing request");
    }

    private SDSRequest parseRequest(HttpServletRequest req) throws IOException {
        BufferedReader ntriples = req.getReader();
        String graph = req.getParameter("graph");
        String resource = req.getParameter("resource");
        String contType = req.getHeader("Content-type");
        String line = null;

        log.trace(String.format("Got resource=%s, graph=%s", resource, graph));
        log.trace(String.format("Content-type: %s", contType));

        log.trace("Dumping N3 file");
        while (( line = ntriples.readLine()) != null)
            log.trace(line);
        log.trace("Done dumping N3 file");

        return new SDSRequest(graph, resource, contType, ntriples);
    }
}