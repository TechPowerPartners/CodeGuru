FROM node:lts-alpine

# Set the working directory
WORKDIR /app

# Install app dependencies
COPY package.json package-lock.json ./

RUN npm install
RUN npm install -g serve

# Bundle app source
COPY . .

# Fix permissions for packages
RUN chown -R node:node /app/node_modules

# Start app
CMD ["npm", "run", "dev"]
