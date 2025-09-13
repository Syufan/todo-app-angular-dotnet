    # build stage 
    FROM node:20-alpine AS build
    WORKDIR /app
    COPY client/package*.json ./
    RUN npm ci
    COPY client/ ./
    # 产物通常在 dist/client/browser
    RUN npm run build
    
    # nginx stage 
    FROM nginx:1.27-alpine
    COPY client/nginx.conf /etc/nginx/conf.d/default.conf
    COPY --from=build /app/dist/client/browser /usr/share/nginx/html
    EXPOSE 8080
    CMD ["nginx","-g","daemon off;"]