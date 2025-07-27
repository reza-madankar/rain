/* eslint-disable */

const env = () => process.env.NODE_ENV;

const get = (key) => process.env[`REACT_APP_${key}`];

export default {
  env,
  get,
};
