
SDShare Push
============

Introduction
------------

SDShare push is a very simple, lightweight protocol that allows
a generic client to push fragments to a receiver. This allows data to be
transferred to just about any kind of service, simply by pointing the
SDShare client at an SDShare push endpoint. 

This document describes the REST interface that a receiver service should
implement.

Interface
---------

The interface is as simple as could be: the SDShare Push Receiver has a
base endpoint (that is, a URL you can reach it at). 

To transfer a fragment, send an HTTP POST request to that URL with two 
URI parameters:

 * `resource`: The URI of the resource represented by the fragment.
   Required.
 * `graph`: An identifier for the collection on the service. Optional,
   may be omitted.

The actual fragment representation is then transmitted in the request
body, with a `Content-type` header indicating the format and character
encoding of the fragment. The `Content-type` header is required.

The service is then expected to update its internal representation of
the resource to match the received fragment.

Semantics:

 * Returning any 2xx code indicates that the fragment was correctly
   received and processed, and that no repost is necessary.

 * A 5xx error indicates that processing failed, and that the client
   cannot rely on the service to have a correct copy of the resource.
   The client may attempt to repost.

 * The service is expected to be idempotent. That is, posting the same
   fragment more than once should have the same effect as submitting
   it once.

Batching
--------

The protocol supports submitting several fragments in one request.
This is done by repeating the `resource` parameter to include the URIs
of all resources in the batch, and by concatenating all fragments into
a single representation.

If the client does not support batching it should fail with a 5xx
error code and a textual message explaining that batching is not
supported.
