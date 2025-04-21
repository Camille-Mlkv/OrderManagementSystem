FROM node:20 AS node
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build --prod

RUN node replace-env.js

FROM nginx:alpine
COPY --from=node /app/dist/angular-client/browser /usr/share/nginx/html