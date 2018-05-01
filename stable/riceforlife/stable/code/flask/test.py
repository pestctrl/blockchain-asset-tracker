import unittest
import run, models, resources
from flask_restful import Resource, reqparse
from models import UserModel, RevokedTokenModel
from flask_jwt_extended import (create_access_token, create_refresh_token, jwt_required, jwt_refresh_token_required, get_jwt_identity, get_raw_jwt)
from flask import jsonify, json


class test(unittest.TestCase):
        def setUp(self):
			self.UserModel = models.UserModel()

        def test_canary(self):
            self.assertTrue(True)

        def test_generateHash(self):
            hash = self.UserModel.generate_hash("hash")
            self.assertTrue(hash)

        def test_verifyHash(self):
            hash = self.UserModel.generate_hash("hash")
            self.assertTrue(self.UserModel.verify_hash("hash",hash))

        def test_getUser(self):
            currentUser = self.UserModel.get_user("Smith")
            self.assertTrue(currentUser)

        def test_revokeToken(self):
            currentUser = UserModel.find_by_username('Smith')
            jti=currentUser.password
            revoked_token=RevokedTokenModel(jti=jti).add()
            self.assertTrue(models.RevokedTokenModel.is_jti_blacklisted(jti=jti))
            self.assertTrue(models.RevokedTokenModel.query.filter_by(jti=jti))





if __name__ == '__main__':
        unittest.main()
