/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {ErrorsInstance, ErrorsAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<ErrorsInstance, ErrorsAttribute>('Errors', {
    ErrorId: {
      type: DataTypes.INTEGER,
      allowNull: false,
      primaryKey: true,
      autoIncrement: true
    },
    IpAddress: {
      type: DataTypes.STRING,
      allowNull: true
    },
    Date: {
      type: DataTypes.DATE,
      allowNull: true
    },
    Message: {
      type: DataTypes.STRING,
      allowNull: true
    },
    StackTrace: {
      type: DataTypes.STRING,
      allowNull: true
    }
  }, {
    tableName: 'Errors',
    timestamps: false
  });
};
