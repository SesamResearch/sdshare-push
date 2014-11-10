import os, rdflib
from sdsharepushreceiver import receiver


class FakeResponse:
    def __init__(self):
        self.content_type = ""


class FakeRequest:
    def __init__(self, resource, graph, body):
        self.params = {"content" : body, "resource" : resource, "graph" : graph}
        self.response = FakeResponse()


def test_request():
    with open("sdsharepushreceiver/testdata/test.nt","r") as inputfile:
        req = FakeRequest("http://psi.test.no/1", "http://psi.test.no/graph",  inputfile.read())
        result = receiver.pushreceiver(req, do_apply=False)

        assert result.status_int == 200
   

if __name__ == '__main__':
    test_request()
