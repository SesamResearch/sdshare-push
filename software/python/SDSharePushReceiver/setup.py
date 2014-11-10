# -*- coding: utf-8 -*-

from setuptools import setup, find_packages

requires = ['chameleon>=2.15', 'pyramid>=1.5', 'pyramid_chameleon','rdflib>=4.1.2','nose']

setup(name='SDSharePushReceiver',
      version='1.0.0',
      description='A SDShare push receiver in Python',
      author='Tom Bech',
      author_email='tom.bech@sesam.io',
      url='http://sesam.io',
      packages=find_packages(),
      include_package_data=True,
      install_requires=requires,
      setup_requires=requires,
      tests_require=requires,
      test_suite = 'nose.collector',
      license = "BSD",
      # Some packages don't deal well with binary eggs...
      zip_safe=False,
      keywords = "sdshare rest receiver http post",
      classifiers=[
        "Development Status :: 4 - Beta",
        "Environment :: Console",
        "Operating System :: MacOS :: MacOS X",
        "Operating System :: Microsoft :: Windows"
        "Operating System :: POSIX",
        "Topic :: Utilities",
        "Topic :: Text Processing :: Markup :: XML",
        "Programming Language :: Python",
        "License :: OSI Approved :: BSD License",
      ],
      entry_points={
          'console_scripts': [
              'sdsharepushreceiver=sdsharepushreceiver.receiver:main',
          ],
      })
