const apiurl = 'https://localhost:7049/api';

export const apiEndpoint = {
  AuthEndpoint: {
    login: `${apiurl}/Account/login`,
    register: `${apiurl}/Account/register`,
  },
  TodoEndpoint: {
    Tasks: `${apiurl}/Tasks`,
    Categories: `${apiurl}/Categories`
  },
};
