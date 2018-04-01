from flask_restful import Resource, reqparse
from models import UserModel, RevokedTokenModel
from flask_jwt_extended import (create_access_token, create_refresh_token, jwt_required, jwt_refresh_token_required, get_jwt_identity, get_raw_jwt)
from flask import jsonify, json
import requests



registerParser = reqparse.RequestParser()
registerParser.add_argument('username', help = 'This field cannot be blank', required = True)
registerParser.add_argument('firstName', help = 'This field cannot be blank', required = True)
registerParser.add_argument('lastName', help = 'This field cannot be blank', required = True)
registerParser.add_argument('password', help = 'This field cannot be blank', required = True)

loginParser = reqparse.RequestParser()
loginParser.add_argument('username', help = 'This field cannot be blank', required = True)
loginParser.add_argument('password', help = 'This field cannot be blank', required = True)


class UserRegistration(Resource):
    def post(self):
        data = registerParser.parse_args()

        if UserModel.find_by_username(data['username']):
            return {'message': 'User {} already exists'. format(data['username'])}

        new_user = UserModel(
            username = data['username'],
            password = UserModel.generate_hash(data['password'])
        )


        try:
            new_user.save_to_db()
            access_token = create_access_token(identity = data['username'])
            refresh_token = create_refresh_token(identity = data['username'])
            print (data['username'])
            url='http://129.213.108.205:3000/api/org.acme.biznet.Trader'
            payload={"$class": "org.acme.biznet.Trader", "traderId": data['username'], "firstName": data['firstName'], "lastName": data['lastName'] }
            headers={'Content-Type': 'application/json'}
            response = requests.post(url, data=json.dumps(payload), headers=headers)


            return {
                'message': 'User {} was created'.format( data['username'])
            }
        except:
            return {'message': 'Something went wrong'}, 500


class UserLogin(Resource):
    def post(self):
        data = loginParser.parse_args()
        current_user = UserModel.find_by_username(data['username'])
        if not current_user:
            return {'message': 'User {} doesn\'t exist'.format(data['username'])}

        if UserModel.verify_hash(data['password'], current_user.password):
            access_token = create_access_token(identity = data['username'])
            refresh_token = create_refresh_token(identity = data['username'])
            return {
            'message': 'Logged in as {}'.format(current_user.username),
            'access_token': access_token,
            'refresh_token': refresh_token
            }
        else:
            return {'message': 'Wrong credentials'}


class UserLogoutAccess(Resource):
    @jwt_required
    def post(self):
        jti = get_raw_jwt()['jti']
        try:
            revoked_token = RevokedTokenModel(jti = jti)
            revoked_token.add()
            return{'message': 'Access token has been revoked'}
        except:
            return{'message': 'Something went wrong'}, 500



class UserLogoutRefresh(Resource):
    @jwt_refresh_token_required
    def post(self):
        jti = get_raw_jwt()['jti']
        try:
            revoked_token = RevokedTokenModel(jti = jti)
            revoked_token.add()
            return{'message': 'Refresh token has been revoked'}
        except:
            return {'message': 'Something went wrong'}, 500

class TokenRefresh(Resource):
    @jwt_refresh_token_required
    def post(self):
        current_user = get_jwt_identity()
        access_token = create_access_token(identity = current_user)
        return {'access_token': access_token}


class AllUsers(Resource):
    def get(self):
        return UserModel.return_all()

    def delete(self):
        return UserModel.delete_all()


class SecretResource(Resource):
    @jwt_required
    def get(self):
        traderId=get_jwt_identity()
        resp =  requests.get('http://129.213.108.205:3000/api/queries/MyAssets?ownerParam=resource%3Aorg.acme.biznet.Trader%23' + traderId)
        return (resp.json())
