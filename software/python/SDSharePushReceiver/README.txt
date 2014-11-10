A Sdshare Push Receiver
=======================

A web server that receives triples by POST and does stuff with it

Installation
------------

This package requires python 3.4 to be installed

In a virtualenv as a normal user so you don't "pollute" your site-packages:
    
    * virtualenv venv
    * source venv/bin/activate (
    * python setup.py install

Or as root/superuser:
    * python setup.py install

To run
------

  * sdsharepushreceiver

The server is running at the port 6547 by default. You can modify the port by command line options.
Run it with --help to see all options.

You can do manual tests by posting NTriples using the form found at: http://localhost:6543/form
