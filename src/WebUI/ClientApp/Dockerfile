FROM node:12
EXPOSE 3000
WORKDIR /usr/src/app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm build
ENTRYPOINT ["npm", "run", "start:dev"]
