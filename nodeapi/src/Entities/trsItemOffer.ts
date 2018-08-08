/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {trsItemOfferInstance, trsItemOfferAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<trsItemOfferInstance, trsItemOfferAttribute>('trsItemOffer', {
    id: {
      type: DataTypes.INTEGER,
      allowNull: false,
      primaryKey:true
    },
    offerName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    startDate: {
      type: DataTypes.DATE,
      allowNull: true
    },
    startTime: {
      type: DataTypes.DATE,
      allowNull: true
    },
    endDate: {
      type: DataTypes.DATE,
      allowNull: true
    },
    endTime: {
      type: DataTypes.DATE,
      allowNull: true
    },
    Itemid: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    batch: {
      type: DataTypes.STRING,
      allowNull: true
    },
    mrp: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    rate: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    Discount: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    netRate: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    deActive: {
      type: DataTypes.BOOLEAN,
      allowNull: true
    }
  }, {
    tableName: 'trsItemOffer'
  });
};
