/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {mtrAccountGroupInstance, mtrAccountGroupAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<mtrAccountGroupInstance, mtrAccountGroupAttribute>('mtrAccountGroup', {
    AccountGroupid: {
      type: DataTypes.BIGINT,
      allowNull: false,
      primaryKey: true
    },
    GroupName: {
      type: DataTypes.STRING,
      allowNull: false
    },
    BaseGroup: {
      type: DataTypes.STRING,
      allowNull: true
    },
    deActive: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    }
  }, {
    tableName: 'mtrAccountGroup'
  });
};
