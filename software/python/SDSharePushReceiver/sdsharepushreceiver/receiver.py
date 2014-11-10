# coding: utf-8

from wsgiref.simple_server import make_server
from pyramid.config import Configurator
from pyramid.response import Response
from pyramid.view import view_config
import rdflib, argparse, logging


logger = None


def applyFragment(resource_uri, graph_uri, ntriples):
    """ Do something with the data here - this is what you will be overriding/changing """
    global logger

    if logger:
        logger.info("Applying fragment...")
    
    # Lets have fun with the graph!
    graph = rdflib.Graph()
    graph.parse(data=ntriples, format="nt")

    # You can traverse the RDF graph using objects and a filter expression...
    subject_filter = rdflib.term.URIRef(resource_uri)
    for subject, predicate, obj in graph.triples((subject_filter, None, None)):
        print(subject, predicate, obj)

    # Alternatively, RDFlib also supports doing Sparql queries!
    query = """
    SELECT DISTINCT ?subject ?predicate ?object
    WHERE {
        ?subject ?predicate ?object
    }"""
    
    results = graph.query(query) 
    
    for row in results:
        print("%s %s %s" % row)
 

@view_config(route_name='form', request_method='GET', renderer='templates/form.pt')
def form(request):
    """ Just a simple POST form so we can debug our code easily """
    return dict(title='Post graph')


@view_config(route_name='pushreceiver', request_method='POST', renderer='string')
def pushreceiver(request, do_apply=True):
    """ The POST receiver handler """
    global logger

    if 'content' in request.params:
        # NTriples from a form post
        ntriples = request.params['content']
    else:
        # NTriples from a SDShare push POST, it can be empty
        ntriples = request.body

    resource_uri = request.params.get('resource')
    graph_uri = request.params.get('graph')

    # Both parameters must be present according to the spec..
    if resource_uri is None or graph_uri is None:
        msg = 'Missing subject or graph parameters in request!'
        if logger:
            logger.error(msg)
        response =  Response(msg)
        response.status_int = 500
    else:
        # All is well, apply the data
        if logger:
            logger.info("Called with resource='%s', graph='%s'" % (resource_uri, graph_uri))
            logger.debug("NTriples: \n%s" % ntriples)
    
        if do_apply:
            applyFragment(resource_uri, graph_uri, ntriples)
        
        response = Response('OK')
        
    return response


def main():
    """ Create a SDShare Push Receiver server and run it forever until stopped """
    
    # Parse arguments
    parser = argparse.ArgumentParser(description="SDShare Push Receiver")
    parser.add_argument("-i", "--interface", dest="interface",
                        help="Interface bind to, default is 127.0.0.1", default="127.0.0.1")    
    parser.add_argument("-p", "--port", type=int, dest="port",
                        help="IP port to bind to, default is 6543", metavar="PORT", default="6543")
    parser.add_argument("-l", "--loglevel", dest="loglevel",
                        help="Loglevel (INFO, DEBUG, WARN..), default is INFO", metavar="LOGLEVEL", default="INFO")
    parser.add_argument("-f", "--logfile", dest="logfile", default="sdsharepushreceiver.log",
                        help="Filename to log to if logging to file, the default is 'sdsharepushreceiver.log' in the current directory")

    options = parser.parse_args()

    # Set up logging
    format_string = '%(asctime)s - %(name)s - %(levelname)s - %(message)s'
    logger = logging.getLogger('sdsharepushreceiver')

    # Set log level (mat to constants)
    logger.setLevel({"INFO":logging.INFO, "DEBUG":logging.DEBUG, "WARN":logging.WARNING, "ERROR":logging.ERROR}.get(options.loglevel))

    # Log to stdout and file
    stdout_handler = logging.StreamHandler()
    stdout_handler.setFormatter(logging.Formatter(format_string))
    logger.addHandler(stdout_handler)
    file_handler = logging.FileHandler(options.logfile)
    file_handler.setFormatter(logging.Formatter(format_string))
    logger.addHandler(file_handler)
    
    # Create a WSGI HTTP server and run it forever
    from pyramid.config import Configurator
    pyramid_config = Configurator()
    pyramid_config.include('pyramid_chameleon')
    pyramid_config.add_route('pushreceiver', '/')
    pyramid_config.add_route('form', '/form')
    pyramid_config.scan()

    server = make_server(options.interface, options.port, pyramid_config.make_wsgi_app() )
    logger.info('Starting up server on http://%s:%s' % (options.interface, options.port))
    server.serve_forever()


# Check if program called from the command line
if __name__ == '__main__':
    main()
