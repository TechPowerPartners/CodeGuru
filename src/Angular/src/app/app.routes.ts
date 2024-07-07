import { Routes } from '@angular/router';
import { TrainingComponent } from './training/training.component';
import { ArticlesComponent } from './articles/articles.component';
import { VacanciesComponent } from './vacancies/vacancies.component';

export const routes: Routes = [
  {
    path: '', component: TrainingComponent
  },
  {
    path: 'articles', component: ArticlesComponent
  },
  {
    path: 'training', component: TrainingComponent
  },
  {
    path: 'vacancies', component: VacanciesComponent
  }
];
