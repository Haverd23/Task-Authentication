FROM node:20 AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . ./
RUN npm run build --prod

FROM nginx:alpine AS runtime
COPY --from=build /app/dist/task-ui/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
COPY mime.types /etc/nginx/mime.types
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]