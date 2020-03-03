import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ErrorHandlerService } from 'app/core/error-handler.service';

@Injectable()
export class SystemService {
    constructor(
        private http: HttpClient,
        private errorHandlerService: ErrorHandlerService
    ) {}

    /**
     * Загружает пользовательские настройки.
     * @param key Ключ к которому относятся настройки.
     */
    loadUserSettings(key: string) {
        return this.http
            .get(`/api/System/LoadUserSettings/${key}`)
            .catch(error => this.errorHandlerService.handleError.call(this.errorHandlerService, error))
            .map(data => data);
    }

    /**
     * Сохраняет пользовательские настройки.
     * @param key Ключ к которому относятся настройки.
     * @param value Значение настроек в формате JSON.
     */
    saveUserSettings(key: string, value: any) {
        return this.http
            .post('/api/System/SaveUserSettings', { key, value: JSON.stringify(value) })
            .catch(error => this.errorHandlerService.handleError.call(this.errorHandlerService, error))
            .map(data => data);
    }

    /**
     * Возвращает текущую версию сборки.
     */
    getVersion() {
        return this.http
            .get('/api/System/Version')
            .catch(error => this.errorHandlerService.handleError.call(this.errorHandlerService, error))
            .map(data => data);
    }
}
