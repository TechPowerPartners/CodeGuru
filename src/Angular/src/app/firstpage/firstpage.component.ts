import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TuiActiveZoneModule, tuiIsString } from '@taiga-ui/cdk';
import {
  TuiDataListModule,
  TuiHostedDropdownModule,
  TuiSvgModule,
} from '@taiga-ui/core';
import { TuiTabsModule } from '@taiga-ui/kit';

@Component({
  selector: 'app-firstpage',
  standalone: true,
  imports: [
    TuiTabsModule,
    TuiHostedDropdownModule,
    TuiSvgModule,
    TuiDataListModule,
    TuiActiveZoneModule,
    TuiHostedDropdownModule,
    CommonModule,
    RouterLink,
  ],
  templateUrl: './firstpage.component.html',
  styleUrl: './firstpage.component.less',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FirstpageComponent {
  readonly collaborators = ['Carol Cleveland', 'Neil Innes'];

  readonly tabs = ['Статьи', 'Обучение', 'Вакансии'];

  activeElement = String(this.tabs[0]);

  get activeItemIndex(): number {
    return this.tabs.indexOf(this.activeElement);
  }

  stop(event: Event): void {
    // We need to stop tab custom event so parent component does not think its active
    event.stopPropagation();
  }

  onClick(activeElement: string): void {
    this.activeElement = activeElement;
  }

  isString(tab: unknown): tab is string {
    return tuiIsString(tab);
  }
}
