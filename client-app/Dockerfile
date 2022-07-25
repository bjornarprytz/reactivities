FROM node:16

WORKDIR /usr/src/app

# Install dependencies
COPY package*.json ./
RUN npm install

# Bundle app source
COPY . ./
RUN npm run build

EXPOSE 3000

CMD [ "npm", "start" ]