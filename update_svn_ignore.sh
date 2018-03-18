#!/bin/bash

svn propset svn:ignore "$(cat .svnignore)" .
