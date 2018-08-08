/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {mtrloyaltyInstance, mtrloyaltyAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<mtrloyaltyInstance, mtrloyaltyAttribute>('mtrloyalty', {
    loyaltyID: {
      type: DataTypes.INTEGER,
      allowNull: false,
      primaryKey: true
    },
    loyaltyName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    saleAmount: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    loyaltyPoint: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    targetPoint: {
      type: DataTypes.FLOAT,
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
    tableName: 'mtrloyalty'
  });
};
